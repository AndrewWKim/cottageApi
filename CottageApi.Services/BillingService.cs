using AutoMapper;
using CottageApi.Core.Configurations;
using CottageApi.Core.Domain.Dto.Billings;
using CottageApi.Core.Domain.Entities;
using CottageApi.Core.Enums;
using CottageApi.Core.Exceptions;
using CottageApi.Core.Extensions;
using CottageApi.Core.Helpers;
using CottageApi.Core.Services;
using CottageApi.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CottageApi.Services
{
	public class BillingService : IBillingService
	{
		private readonly IMapper _mapper;
		private readonly ILogger _logger;

		private readonly ICottageDbContext _cottageDbContext;
		private readonly IBankPivdenniyService _pivdenniyService;

		public int VerifyCount = 0;

		public BillingService(
			IMapper mapper,
			ILogger<BillingService> logger,
			ICottageDbContext cottageDbContext,
			Config config,
			IBankPivdenniyService pivdenniyService)
		{
			_mapper = mapper;
			_logger = logger;
			_cottageDbContext = cottageDbContext;
			_pivdenniyService = pivdenniyService;
		}

		public async Task<IEnumerable<CottageBilling>> GetCottageBillingsAsync(int userId)
		{
			var client = await _cottageDbContext.Clients.FirstOrDefaultAsync(c => c.UserId == userId);
			var cottageBillings = await _cottageDbContext.CottageBillings.Where(cb => cb.CottageId == client.CottageId).Include(cb => cb.CommunalBills).ToListAsync();

			return cottageBillings;
		}

		public async Task<IEnumerable<CottageBilling>> GetCottageBillingsForMeterAsync(int userId)
		{
			var client = await _cottageDbContext.Clients.FirstOrDefaultAsync(c => c.UserId == userId);
			var cottageBillings = await _cottageDbContext.CottageBillings
				.Include(cb => cb.CommunalBills)
				.Where(cb => cb.CottageId == client.CottageId && cb.CommunalBills.Any(comBill => comBill.CommunalType != CommunalTypesUA.GetCommunalType("ok")))
				.Select(cb => new CottageBilling()
				{
					Id = cb.Id,
					BillingDate = cb.BillingDate.AddMonths(-1),
					CottageId = cb.CottageId,
				})
				.ToListAsync();

			return cottageBillings;
		}

		public async Task<IEnumerable<CommunalBill>> GetCommunalBillsAsync(int cottageBillingId)
		{
			var communalBills = await _cottageDbContext.CommunalBills.Where(cb => cb.CottageBillingId == cottageBillingId).ToListAsync();

			return communalBills;
		}

		public async Task<Tuple<IEnumerable<CommunalBill>, int>> GetCommunalBillsForAdminAsync(CommunalBillFilter filter)
		{

			IQueryable<CommunalBill> query = _cottageDbContext.CommunalBills
                .Include(c => c.CottageBilling)
                    .ThenInclude(cb => cb.Cottage)
                        .ThenInclude(cottage => cottage.Clients);

			if (filter.CottageId.HasValue)
			{
				query = query.Where(c => c.CottageBilling.CottageId == filter.CottageId.Value);
			}

			if (filter.Month.HasValue)
			{
				query = query.Where(c => c.CottageBilling.BillingDate.Month == filter.Month.Value);
			}

			if (filter.Year.HasValue)
			{
				query = query.Where(c => c.CottageBilling.BillingDate.Year == filter.Year.Value);
			}

			int total = query.Count();

			var communalBills = await query
				.OrderByDescending(c => c.CottageBilling.BillingDate)
				.Paging(filter.Offset, filter.Limit)
				.ToListAsync();

			return new Tuple<IEnumerable<CommunalBill>, int>(communalBills, total);
		}

		public async Task<int> GetUnpaidCommunalBillsCountAsync(int userId)
		{
			var client = await _cottageDbContext.Clients.FirstOrDefaultAsync(c => c.UserId == userId);
			var communalBills = await _cottageDbContext.CottageBillings
				.Include(cb => cb.CommunalBills)
				.Where(cb => cb.CottageId == client.CottageId)
				.SelectMany(cb => cb.CommunalBills)
				.Where(communalBill => communalBill.PaymentStatus == PaymentStatus.Unpaid)
				.ToListAsync();

			var unpaidCommunalBillsCount = communalBills.Count();
			return unpaidCommunalBillsCount;
		}

		public async Task<IEnumerable<CommunalBill>> GetPayedCommunalBillsAsync(DateTime from, DateTime to)
		{
			var cottageBillings = await _cottageDbContext.CottageBillings
				.Where(cb => cb.BillingDate >= from && cb.BillingDate <= to)
				.Select(cb => cb.Id)
				.ToListAsync();

			var payedCommunalBills = await _cottageDbContext.CommunalBills
				.Where(comBill => comBill.PaymentStatus == PaymentStatus.Paid && cottageBillings.Contains(comBill.CottageBillingId))
				.ToListAsync();

			return payedCommunalBills;
		}

		public async Task CreateCottageBillingsAsync(IEnumerable<CreateCottageBillingDto> cottageBillingsDto)
		{
			var cottageBillings = new List<CottageBilling>();

			foreach (var cottageBillingDto in cottageBillingsDto)
			{
				if (string.IsNullOrWhiteSpace(cottageBillingDto.ClientItn))
				{
					continue;
				}

				var client = await _cottageDbContext.Clients.AsNoTracking().FirstOrDefaultAsync(c => c.ITN == cottageBillingDto.ClientItn);

				if (client == null)
				{
					throw new BusinessException($"Клиента с ИНН {cottageBillingDto.ClientItn} не найдено.");
				}

				var existingCottageBilling = await _cottageDbContext.CottageBillings.AsNoTracking().FirstOrDefaultAsync(cb =>
					cb.CottageId == client.CottageId
						&& cb.BillingDate.Month == cottageBillingDto.BillingDate.Month
						&& cb.BillingDate.Year == cottageBillingDto.BillingDate.Year);

				var cottageBilling = new CottageBilling()
				{
					BillingDate = cottageBillingDto.BillingDate,
					CottageId = client.CottageId,
					CommunalBills = _mapper.Map<List<CommunalBill>>(cottageBillingDto.CommunalBills),
				};

				if (existingCottageBilling != null)
				{
					await EditExistingCottageBillingAsync(existingCottageBilling.Id, cottageBilling);
					continue;
				}

				cottageBillings.Add(cottageBilling);
			}

			_cottageDbContext.CottageBillings.AddRange(cottageBillings);
			await _cottageDbContext.SaveChangesAsync();
		}

		private async Task EditExistingCottageBillingAsync(int existingCottageBillingId, CottageBilling cottageBillingToEdit)
		{
			var existingCottageBilling = await _cottageDbContext.CottageBillings.Include(cb => cb.CommunalBills).FirstOrDefaultAsync(cb => cb.Id == existingCottageBillingId);
			var existingCommunalBills = existingCottageBilling.CommunalBills.ToList();

			foreach (var createCommunalBillDto in cottageBillingToEdit.CommunalBills)
			{
				var existingCommunalBill = existingCommunalBills.FirstOrDefault(comBill => comBill.BillGUID == createCommunalBillDto.BillGUID && comBill.CommunalType == createCommunalBillDto.CommunalType);
				if (existingCommunalBill != null)
				{
					existingCommunalBill.Price = createCommunalBillDto.Price;
					existingCommunalBill.PaymentStatus = createCommunalBillDto.PaymentStatus;
					existingCommunalBill.MeterDataBegin = createCommunalBillDto.MeterDataBegin;
					existingCommunalBill.MeterDataEnd = createCommunalBillDto.MeterDataEnd;
					existingCommunalBill.MeterData = createCommunalBillDto.MeterDataEnd - createCommunalBillDto.MeterDataBegin;
					continue;
				}

				var newCommunalBill = _mapper.Map<CommunalBill>(createCommunalBillDto);
				existingCommunalBills.Add(newCommunalBill);
			}

			existingCottageBilling.CommunalBills = existingCommunalBills;
			_cottageDbContext.CottageBillings.Update(existingCottageBilling);
			await _cottageDbContext.SaveChangesAsync();
		}

		public async Task ChangePaymentStatusAsync(PaymentType paymentType, int paymentId, PaymentStatus paymentStatus)
		{
			switch (paymentType)
			{
				case PaymentType.CommunalBill:
					await UpdateCommunalBillPaymentStatus(paymentId, paymentStatus);
					break;
				case PaymentType.CottageBilling:
					await UpdateCottageBillingPaymentStatus(paymentId, paymentStatus);
					break;
				default:
					break;
			}
		}






		public async Task ProcessPivdenniyPaymentResponseAsync(PivdenniyPaymentResponse response)
		{
			await _pivdenniyService.CreatePaymentResponseAsync(response);
			var paymentEffort = await _cottageDbContext.PivdenniyPaymentEfforts.AsNoTracking().FirstOrDefaultAsync(ppe => ppe.OrderId == response.OrderID);
			await CreateOrUpdateClientCardAsync(paymentEffort.ClientId, response);

			PaymentStatus status = PaymentStatus.Unpaid;

			var communalBills = await _cottageDbContext.CommunalBills
				.Include(cb => cb.PivdenniyPaymentEfforts)
				.Where(cb => cb.PivdenniyPaymentEfforts.Any(ppe => ppe.OrderId == response.OrderID)).ToListAsync();

			switch (response.TranCode)
			{
				case "000":
					status = PaymentStatus.Paid;
					break;
				default:
					break;
			}

			await UpdateCommunalBillsPaymentStatusAsync(communalBills, status);
		}


		public async Task<PaymentDataDto> GetCardAddingHTMLAsync(PaymentType paymentType, int clientId)
		{
			var client = await _cottageDbContext.Clients.FirstOrDefaultAsync(c => c.Id == clientId);

			//Update payCount on every request to generate new orderId every time
			var payEffort = await IncreaseClientRequestTryCountAsync(client);
			var orderId = await GenerateUniqueOrderIdAsync(int.Parse($"{payEffort}{client.Id}"));

			var newPaymentEffort = new PivdenniyPaymentEffort()
			{
				OrderId = orderId,
				ClientId = clientId,
				CommunalBillId = null
			};
			_cottageDbContext.PivdenniyPaymentEfforts.Add(newPaymentEffort);
			await _cottageDbContext.SaveChangesAsync();

			var paymentDescription = new PaymentDescription()
			{
				ClientFirstName = client.FirstName,
				ClientLastName = client.LastName,
				PaymentType = paymentType,
				BillingName = "Добавление карты",
				Month = 0,
				Year = 0,
				BillingId = 0
			};

			var paymentData = _pivdenniyService.GetCardAddingData(orderId, client.Id, paymentDescription);

			return paymentData;
		}

		public async Task<PaymentDataDto> GetPaymentHTMLAsync(PaymentType paymentType, int clientId, int? billingId)
		{
			var client = await _cottageDbContext.Clients.FirstOrDefaultAsync(c => c.Id == clientId);

			//Update payCount on every request to generate new orderId every time
			var payEffort = await IncreaseClientRequestTryCountAsync(client);
			var orderId = await GenerateUniqueOrderIdAsync(int.Parse($"{payEffort}{client.Id}"));

			IQueryable<CommunalBill> query = _cottageDbContext.CommunalBills
				.Include(cb => cb.CottageBilling)
				.Where(cb => cb.PaymentStatus == PaymentStatus.Unpaid && cb.CottageBilling.CottageId == client.CottageId);

			switch (paymentType)
			{
				case PaymentType.CommunalBill:
					query = query.Where(cb => cb.Id == billingId);
					break;
				case PaymentType.CottageBilling:
					query = query.Where(cb => cb.CottageBillingId == billingId);
					break;
				case PaymentType.All:
					break;
				default:
					break;
			}

			var total = query.Count();

			if (total <= 0)
			{
				throw new ValidationException("billingId", "Уже в обработке или оплачено.");
			}

			var communalBills = await query.ToListAsync();

			await UpdateCommunalBillsPaymentStatusAsync(communalBills, PaymentStatus.InProcess);

			var amount = 0;

			var paymentDescription = new PaymentDescription()
			{
				ClientFirstName = client.FirstName,
				ClientLastName = client.LastName,
				PaymentType = paymentType,
				BillingName = "Вся оставшаяся сумма",
				Month = 0,
				Year = 0,
				BillingId = billingId.HasValue ? billingId.Value : 0
			};

			if (paymentType == PaymentType.CommunalBill)
			{
				var communalBill = communalBills.FirstOrDefault();
				amount = (int)(communalBill.Price * 100);
				paymentDescription.Month = communalBill.CottageBilling.BillingDate.Month;
				paymentDescription.Year = communalBill.CottageBilling.BillingDate.Year;
				paymentDescription.BillingName = CastUACommunalTypeForBillId(communalBill.CommunalType);
			}
			else if (paymentType == PaymentType.CottageBilling)
			{
				amount = (int)(communalBills.Sum(cb => cb.Price) * 100);
				var communalBill = communalBills.FirstOrDefault();
				paymentDescription.Month = communalBill.CottageBilling.BillingDate.Month;
				paymentDescription.Year = communalBill.CottageBilling.BillingDate.Year;
				paymentDescription.BillingName = "Остаток за месяц";
			}
			else if (paymentType == PaymentType.All)
			{
				amount = (int)(communalBills.Sum(cb => cb.Price) * 100);
			}

			foreach (var cb in communalBills)
			{
				var newPaymentEffort = new PivdenniyPaymentEffort()
				{
					OrderId = orderId,
					CommunalBillId = cb.Id,
					ClientId = clientId,
				};
				_cottageDbContext.PivdenniyPaymentEfforts.Add(newPaymentEffort);
				await _cottageDbContext.SaveChangesAsync();
			}

			//COMISSION
			amount = (int)Math.Floor(amount / 0.975);

			//amount = amount + (int)(amount * 0.02565);

			var paymentData = _pivdenniyService.GetPaymentData(orderId, client.Id, amount, paymentDescription);

			return paymentData;
		}

		public async Task<PaymentDataDto> MakePaymentWithTokenAsync(PaymentType paymentType, int clientId, int cardId, int? billingId)
		{
			var client = await _cottageDbContext.Clients.FirstOrDefaultAsync(c => c.Id == clientId);

			//Update payCount on every request to generate new orderId every time
			var payEffort = await IncreaseClientRequestTryCountAsync(client);
			var amount = 0;
			var orderId = await GenerateUniqueOrderIdAsync(int.Parse($"{payEffort}{client.Id}"));

			IQueryable<CommunalBill> query = _cottageDbContext.CommunalBills
				.Include(cb => cb.CottageBilling)
				.Where(cb => cb.PaymentStatus == PaymentStatus.Unpaid && cb.CottageBilling.CottageId == client.CottageId);

			switch (paymentType)
			{
				case PaymentType.CommunalBill:
					query = query.Where(cb => cb.Id == billingId);
					break;
				case PaymentType.CottageBilling:
					query = query.Where(cb => cb.CottageBillingId == billingId);
					break;
				case PaymentType.All:
					break;
				default:
					break;
			}

			var total = query.Count();

			if (total <= 0)
			{
				throw new ValidationException("billingId", "Уже в обработке или оплачено.");
			}

			var communalBills = await query.ToListAsync();

			foreach (var cb in communalBills)
			{
				var newPaymentEffort = new PivdenniyPaymentEffort()
				{
					OrderId = orderId,
					ClientId = clientId,
					CommunalBillId = cb.Id
				};
				_cottageDbContext.PivdenniyPaymentEfforts.Add(newPaymentEffort);
				await _cottageDbContext.SaveChangesAsync();
			}

			await UpdateCommunalBillsPaymentStatusAsync(communalBills, PaymentStatus.InProcess);

			var paymentDescription = new PaymentDescription()
			{
				ClientFirstName = client.FirstName,
				ClientLastName = client.LastName,
				PaymentType = paymentType,
				BillingName = "Вся оставшаяся сумма",
				Month = 0,
				Year = 0,
				BillingId = billingId.HasValue ? billingId.Value : 0
			};

			if (paymentType == PaymentType.CommunalBill)
			{
				var communalBill = communalBills.FirstOrDefault();
				amount = (int)(communalBill.Price * 100);
				paymentDescription.Month = communalBill.CottageBilling.BillingDate.Month;
				paymentDescription.Year = communalBill.CottageBilling.BillingDate.Year;
				paymentDescription.BillingName = CastUACommunalTypeForBillId(communalBill.CommunalType);
			}
			else if (paymentType == PaymentType.CottageBilling)
			{
				amount = (int)(communalBills.Sum(cb => cb.Price) * 100);
				var communalBill = communalBills.FirstOrDefault();
				paymentDescription.Month = communalBill.CottageBilling.BillingDate.Month;
				paymentDescription.Year = communalBill.CottageBilling.BillingDate.Year;
				paymentDescription.BillingName = "Остаток за месяц";
			}
			else if (paymentType == PaymentType.All)
			{
				amount = (int)(communalBills.Sum(cb => cb.Price) * 100);
			}

			var card = await _cottageDbContext.Cards.FirstOrDefaultAsync(c => c.Id == cardId);

			var response = await _pivdenniyService.SendPaymentAsync(orderId, client.Id, amount, card.CardToken, card.PurchaseTime, paymentDescription);

			PaymentStatus newPaymentStatus = PaymentStatus.Unpaid;

			var paymentData = new PaymentDataDto()
			{
				OrderId = orderId,
				HtmlPaymentForm = null,
				PaymentResultStatus = PaymentResultStatus.Fail
			};

			if (response != null)
			{
				_cottageDbContext.PivdenniyPaymentResponses.Add(response);
				await _cottageDbContext.SaveChangesAsync();
			}
			else
			{
				await UpdateCommunalBillsPaymentStatusAsync(communalBills, newPaymentStatus);
				return paymentData;
			}

			if (response.TranCode == "000")
			{
				newPaymentStatus = PaymentStatus.Paid;
				paymentData.PaymentResultStatus = PaymentResultStatus.Success;
			}
			else if (response.TranCode == "414")
			{
				newPaymentStatus = PaymentStatus.Unpaid;
				paymentData.PaymentResultStatus = PaymentResultStatus.Show3DSecure;

				// PIVDENNIY - need to know the correct page, or url where to redirect client
				paymentData.HtmlPaymentForm = "https://www.google.com/";
			}
			else if (response.TranCode == "101")
			{
				paymentData.PaymentResultStatus = PaymentResultStatus.FailedCardTime;
			}

			await UpdateCommunalBillsPaymentStatusAsync(communalBills, newPaymentStatus);
			return paymentData;
		}

		public async Task StartCountdownForIfNoReposnseSetUnpaidAsync(int orderId)
		{
			var communalBills = await _cottageDbContext.CommunalBills.Include(cb => cb.PivdenniyPaymentEfforts).Where(cb => cb.PivdenniyPaymentEfforts.Any(ppe => ppe.OrderId == orderId)).ToListAsync();

			Thread.Sleep(1200000);

			var paymentResponses = await _cottageDbContext.PivdenniyPaymentResponses.AsNoTracking().Where(pr => pr.OrderID == orderId).ToListAsync();

			if (!paymentResponses.Any())
			{
				await UpdateCommunalBillsPaymentStatusAsync(communalBills, PaymentStatus.Unpaid);
			}
		}

		private async Task<int> IncreaseClientRequestTryCountAsync(Client client)
		{
			var payCount = client.PayCount;

			client.PayCount += 1;

			_cottageDbContext.Clients.Update(client);
			await _cottageDbContext.SaveChangesAsync();

			return payCount;
		}

		private string CastUACommunalTypeForBillId(string communalType)
		{
			return communalType == CommunalTypesUA.GetCommunalType("ok")
							? "Обслуживание кооператива"
							: communalType == CommunalTypesUA.GetCommunalType("water")
								? "Водоснабжение"
								: communalType == CommunalTypesUA.GetCommunalType("sewerage")
									? "Канализация"
									: communalType == CommunalTypesUA.GetCommunalType("electricity")
										? "Електроэнергия"
										: "Неизвестно";
		}

		private async Task UpdateCottageBillingPaymentStatus(int paymentId, PaymentStatus paymentStatus)
		{
			var cottageBilling = await _cottageDbContext.CottageBillings.Include(cb => cb.CommunalBills).FirstOrDefaultAsync(cb => cb.Id == paymentId);
			foreach (var communalBill in cottageBilling.CommunalBills)
			{
				communalBill.PaymentStatus = paymentStatus;
			}

			_cottageDbContext.CottageBillings.Update(cottageBilling);
			await _cottageDbContext.SaveChangesAsync();
		}

		private async Task UpdateCommunalBillPaymentStatus(int paymentId, PaymentStatus paymentStatus)
		{
			var communalBill = await _cottageDbContext.CommunalBills.FirstOrDefaultAsync(cb => cb.Id == paymentId);
			communalBill.PaymentStatus = paymentStatus;
			_cottageDbContext.CommunalBills.Update(communalBill);
			await _cottageDbContext.SaveChangesAsync();
		}

		private async Task UpdateCommunalBillsPaymentStatusAsync(List<CommunalBill> communalBills, PaymentStatus paymentStatus)
		{
			foreach (var communalBill in communalBills)
			{
				if (communalBill.PaymentStatus == PaymentStatus.Paid || communalBill.PaymentStatus == PaymentStatus.Partialy)
				{
					continue;
				}
				communalBill.PaymentStatus = paymentStatus;
				_cottageDbContext.CommunalBills.Update(communalBill);
				await _cottageDbContext.SaveChangesAsync();
			}
		}

		private async Task<int> GenerateUniqueOrderIdAsync(int orderId)
		{
			var isEffortExist = await _cottageDbContext.PivdenniyPaymentEfforts.AnyAsync(ppe => ppe.OrderId == orderId);

			if (!isEffortExist)
			{
				return orderId;
			}

			return await GenerateUniqueOrderIdAsync(orderId + 1);
		}

		private async Task CreateOrUpdateClientCardAsync(int clientId, PivdenniyPaymentResponse response)
		{
			var existingCard = await _cottageDbContext.Cards.FirstOrDefaultAsync(c => c.CardPan == response.ProxyPan);

			if (existingCard != null && existingCard.CardToken != response.UPCToken)
			{
				existingCard.CardToken = response.UPCToken;
				existingCard.PurchaseTime = response.PurchaseTime;
				_cottageDbContext.Cards.Update(existingCard);
				await _cottageDbContext.SaveChangesAsync();
				return;
			}

			if ((!string.IsNullOrWhiteSpace(response.ProxyPan) && !string.IsNullOrWhiteSpace(response.UPCToken)) && (existingCard == null || existingCard.ClientId != clientId))
			{
				var newCard = new Card()
				{
					CardPan = response.ProxyPan,
					CardToken = response.UPCToken,
					PurchaseTime = response.PurchaseTime,
					ClientId = clientId
				};

				_cottageDbContext.Cards.Add(newCard);
				await _cottageDbContext.SaveChangesAsync();
			}
		}
	}
}
