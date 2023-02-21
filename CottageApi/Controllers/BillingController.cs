using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CottageApi.Controllers.Base;
using CottageApi.Core.Domain.Dto.Billings;
using CottageApi.Core.Domain.Entities;
using CottageApi.Core.Enums;
using CottageApi.Core.Services;
using CottageApi.Models.Billing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CottageApi.Controllers
{
	[Route("api/[controller]")]
	public class BillingController : BaseApiController
	{
		private readonly IBillingService _billingService;
		private readonly IMapper _mapper;
		private readonly ILogger<BillingController> _logger;

		public BillingController(
			IBillingService billingService,
			IMapper mapper,
			ILogger<BillingController> logger)
		{
			_billingService = billingService;
			_mapper = mapper;
			_logger = logger;
		}

		[HttpGet("cottage/{userId}")]
		public async Task<object> GetCottageBillings([FromRoute]int userId, [FromQuery] bool? isForMeter = false)
		{
			var billnigs = isForMeter.Value
				? await _billingService.GetCottageBillingsForMeterAsync(userId)
				: await _billingService.GetCottageBillingsAsync(userId);

			var billingViews = _mapper.Map<List<CottageBillingViewModel>>(billnigs);
			return billingViews;
		}

		[HttpGet("cottage/unpaid-finance/{userId}")]
		public async Task<int> GetUnpaidCommunalBillsCount([FromRoute]int userId)
		{
			var unpaidCommunalBillsCount = await _billingService.GetUnpaidCommunalBillsCountAsync(userId);

			return unpaidCommunalBillsCount;
		}

		[HttpGet("cottage/bills/{cottageBillingId}")]
		public async Task<object> GetCommunalBills([FromRoute]int cottageBillingId)
		{
			var communalBills = await _billingService.GetCommunalBillsAsync(cottageBillingId);

			var communalViews = _mapper.Map<List<CommunalBillViewModel>>(communalBills);
			return communalViews;
		}

		[HttpGet("communal-bills")]
		public async Task<object> GetAdminCommunalBills(
			[FromQuery]int? offset = null,
			[FromQuery]int? limit = null,
			[FromQuery]int? cottageId = null,
			[FromQuery]int? month = null,
			[FromQuery]int? year = null)
		{
			var filter = new CommunalBillFilter
			{
				Offset = offset,
				Limit = limit,
				CottageId = cottageId,
				Month = month,
				Year = year
			};

			var entities = await _billingService.GetCommunalBillsForAdminAsync(filter);

			var communalBills = _mapper.Map<List<AdminCommunalBillViewModel>>(entities.Item1);

			foreach (var communalBill in communalBills)
            {
                var communalBillEntity = entities.Item1.FirstOrDefault(c => c.Id == communalBill.Id);
                var owner = communalBillEntity.CottageBilling.Cottage.Clients.FirstOrDefault(c => c.ClientType == ClientType.Owner);
                if (owner != null)
                {
                    communalBill.CottageOwner = owner.GetFullName();
				}
            }

			return new
			{
				Total = entities.Item2,
				CommunalBills = communalBills
			};
		}

		[AllowAnonymous]
		[HttpPost]
		public async Task<object> CreateCottageBillings(IEnumerable<CreateCottageBillingModel> cottageBillingModels)
		{
			var cottageBillingsDto = _mapper.Map<List<CreateCottageBillingDto>>(cottageBillingModels);

			await _billingService.CreateCottageBillingsAsync(cottageBillingsDto);
			return Ok();
		}

		[AllowAnonymous]
		[HttpGet("payed")]
		public async Task<IEnumerable<PayedCommunalBillsViewModel>> GetPayedCommunalBills(
			[FromQuery] DateTime from,
			[FromQuery] DateTime to)
		{
			var payedBills = await _billingService.GetPayedCommunalBillsAsync(from, to);
			return _mapper.Map<List<PayedCommunalBillsViewModel>>(payedBills);
		}

		[HttpGet("card/add")]
		public async Task<PaymentDataDto> GetCardAddingPage(
		[FromQuery] PaymentType paymentType,
		[FromQuery] int clientId)
		{
			var cardAddingData = await _billingService.GetCardAddingHTMLAsync(paymentType, clientId);
			return cardAddingData;
		}

		[HttpGet("over-limit/pay")]
		public async Task<PaymentDataDto> GetPaymentPage(
		[FromQuery] PaymentType paymentType,
		[FromQuery] int clientId,
		[FromQuery] int? billingId = null)
		{
			var cardAddingData = await _billingService.GetPaymentHTMLAsync(paymentType, clientId, billingId);
			return cardAddingData;
		}

		[HttpGet("pay")]
		public async Task<PaymentDataDto> PayWithCard(
		[FromQuery] PaymentType paymentType,
		[FromQuery] int clientId,
		[FromQuery] int cardId,
		[FromQuery] int? billingId = null)
		{
			var result = await _billingService.MakePaymentWithTokenAsync(paymentType, clientId, cardId, billingId);
			return result;
		}

		[HttpGet("payment/check")]
		public async Task<object> CheckPayment(
			[FromQuery] int orderId)
		{
			await _billingService.StartCountdownForIfNoReposnseSetUnpaidAsync(orderId);
			return Ok();
		}

		[HttpPut("cottage/payment-status")]
		public async Task<object> ChangePaymentStatus(
			[FromQuery] PaymentType paymentType,
			[FromQuery] int paymentId,
			[FromQuery] PaymentStatus paymentStatus)
		{
			await _billingService.ChangePaymentStatusAsync(paymentType, paymentId, paymentStatus);
			return Ok();
		}

		[AllowAnonymous]
		[HttpPost("pivdenniy/payment")]
		public async Task<string> CreatePaymentresponse([FromForm]PivdenniyPaymentResponse response)
		{
			// If Amount == 1 UAH - than we need to reject this payment, to get only TOKEN without money
			response.ResponseAction = response.TotalAmount == "100" ? "reverse" : "approve";

			await _billingService.ProcessPivdenniyPaymentResponseAsync(response);
			/*var result = $"echo \"MerchantID= {response.MerchantID} \\n\";\necho \"TerminalID= {response.TerminalID} \\n\";\necho \"OrderID= {response.OrderID} \\n\";\necho \"Currency= {response.Currency} \\n\";\necho \"TotalAmount= {response.TotalAmount} \\n\";\necho \"XID= {response.XID} \\n\";\necho \"PurchaseTime= {response.PurchaseTime} \\n\";\necho \"Response.action= {response.ResponseAction} \\n\";\necho \"Response.reason= \\n\";\necho \"Response.forwardUrl= \\n\";";*/

			var result = $"MerchantID='{response.MerchantID}'\nTerminalID='{response.TerminalID}'\nOrderID='{response.OrderID}'\nCurrency='{response.Currency}'\nTotalAmount='{response.TotalAmount}'\nXID='{response.XID}'\nPurchaseTime='{response.PurchaseTime}'\nResponse.action='{response.ResponseAction}'\nResponse.reason=''\nResponse.forwardUrl=''";

			return result;
		}
	}
}
