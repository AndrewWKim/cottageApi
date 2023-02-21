using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CottageApi.Core.Configurations;
using CottageApi.Core.Domain.Dto.Billings;
using CottageApi.Core.Domain.Entities;
using CottageApi.Core.Enums;
using CottageApi.Core.Helpers;
using CottageApi.Core.Services;
using CottageApi.Data.Context;
using CottageApi.Models.Billing;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;


namespace CottageApi.Services
{
	public class BankPivdenniyService : IBankPivdenniyService
	{
		private readonly IMapper _mapper;
		private readonly ILogger _logger;

		private readonly ICottageDbContext _cottageDbContext;

		private readonly PivdenniyBankConfig _pivdenniyBankConfig;

		private readonly HttpClient _httpClient;

		public BankPivdenniyService(
			IMapper mapper,
			ILogger<BankPivdenniyService> logger,
			ICottageDbContext cottageDbContext,
			IHttpClientFactory clientFactory,
			Config config)
		{
			_mapper = mapper;
			_logger = logger;
			_cottageDbContext = cottageDbContext;
			_pivdenniyBankConfig = config.PivdenniyBankConfig;
			_httpClient = clientFactory.CreateClient("Pivdenniy");
		}

		public async Task CreatePaymentResponseAsync(PivdenniyPaymentResponse response)
		{
			_cottageDbContext.PivdenniyPaymentResponses.Add(response);
			await _cottageDbContext.SaveChangesAsync();
		}

		public PaymentDataDto GetCardAddingData(int orderId, int clientId, PaymentDescription description)
		{
			var descriptionString = GeneratePaymentDescription(description);

			var today = DateTime.Now;
			var orderDate = $"{today.Year.ToString().Substring(2)}{DateConverter.TwoDigitsDateValue(today.Month)}{DateConverter.TwoDigitsDateValue(today.Day)}{DateConverter.TwoDigitsDateValue(today.Hour)}{DateConverter.TwoDigitsDateValue(today.Minute)}{DateConverter.TwoDigitsDateValue(today.Second)}";

			var stringToSign = $"{_pivdenniyBankConfig.MerchantID};{_pivdenniyBankConfig.TerminalID};{orderDate};{orderId};{_pivdenniyBankConfig.Currency};100;;";
			var signature = PemSignature.SignStringWithPrivatePem(stringToSign);

			var paymentData = new PaymentDataDto()
			{
				OrderId = orderId,
				HtmlPaymentForm = $"<html><head></head><body style=\"margin: 0; font-size: 4rem;\"><div style=\"display: flex; justify-content: center; align-items: center; height: 100%; flex-direction: column; background: #6B4F9E; padding: 10%\"><div style=\"text-align: center; margin-bottom: 4vh; color: white\"> После добавления карточки, сообщение об ошибке говорит об <strong> успешной </strong> операции. <br /><br /> Во время добавления карточки, у вас будет заблокированна сумма в размере 1 грн. После операции она будет возвращена.<br /></div><form action=\"{_pivdenniyBankConfig.PaymentUrl}\" method=\"POST\" style=\"width:100%\"><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\"><input name=\"Version\" type=\"hidden\" value=\"1\"/><input name=\"MerchantID\" type=\"hidden\" value=\"{_pivdenniyBankConfig.MerchantID}\"/><input name=\"TerminalID\" type=\"hidden\" value=\"{_pivdenniyBankConfig.TerminalID}\"/><input name=\"TotalAmount\" type=\"hidden\" value=\"100\"/><input name=\"Currency\" type=\"hidden\" value=\"{_pivdenniyBankConfig.Currency}\"/><input name=\"PurchaseTime\" type=\"hidden\" value=\"{orderDate}\"/><input name=\"Locale\" type=\"hidden\" value=\"{_pivdenniyBankConfig.Locale}\"/><input name=\"OrderID\" type=\"hidden\" value=\"{orderId}\"/><input name=\"PurchaseDesc\" type=\"hidden\" value=\"{descriptionString}\"/><input name=\"Signature\" type=\"hidden\" value=\"{signature}\"/><button type=\"submit\" style=\"background: #202020; border: 1px solid #202020; color: white; padding: 3rem 1rem; font-weight: bold; width: 100%; font-size: 3rem; margin-top: 20px\">продолжить</button></form></div></body></html>",
				PaymentResultStatus = PaymentResultStatus.Success
			};

			return paymentData;
		}

		public PaymentDataDto GetPaymentData(int orderId, int clientId, int amount, PaymentDescription description)
		{
			var descriptionString = GeneratePaymentDescription(description);

			var today = DateTime.Now;
			var orderDate = $"{today.Year.ToString().Substring(2)}{DateConverter.TwoDigitsDateValue(today.Month)}{DateConverter.TwoDigitsDateValue(today.Day)}{DateConverter.TwoDigitsDateValue(today.Hour)}{DateConverter.TwoDigitsDateValue(today.Minute)}{DateConverter.TwoDigitsDateValue(today.Second)}";

			var stringToSign = $"{_pivdenniyBankConfig.MerchantID};{_pivdenniyBankConfig.TerminalID};{orderDate};{orderId};{_pivdenniyBankConfig.Currency};{amount};;";
			var signature = PemSignature.SignStringWithPrivatePem(stringToSign);

			var paymentData = new PaymentDataDto()
			{
				OrderId = orderId,
				HtmlPaymentForm = $"<div style=\"display: flex; justify-content: center; align-items: center; height: 100%; flex-direction: column; font-size: 20px; color:\"white\"\"><div style=\"text-align: center; width: 70%; margin-bottom: 40px;\">Для оплаты суммы больше 5000 грн, вам необходимо будет ввести реквизиты карты заново<br /></div><form action=\"{_pivdenniyBankConfig.PaymentUrl}\" method=\"POST\"><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\"><input name=\"Version\" type=\"hidden\" value=\"1\" /><input name=\"MerchantID\" type=\"hidden\" value=\"{_pivdenniyBankConfig.MerchantID}\" /><input name=\"TerminalID\" type=\"hidden\" value=\"{_pivdenniyBankConfig.TerminalID}\" /><input name=\"TotalAmount\" type=\"hidden\" value=\"{amount}\" /><input name=\"Currency\" type=\"hidden\" value=\"{_pivdenniyBankConfig.Currency}\" /><input name=\"PurchaseTime\" type=\"hidden\" value=\"{orderDate}\" /><input name=\"Locale\" type=\"hidden\" value=\"{_pivdenniyBankConfig.Locale}\" /><input name=\"OrderID\" type=\"hidden\" value=\"{orderId}\" /><input name=\"PurchaseDesc\" type=\"hidden\" value=\"{descriptionString}\" /><input name=\"Signature\" type=\"hidden\" value=\"{signature}\" /><button type=\"submit\" style=\"background: \"#202020\"; border: none; color: white; padding: 15px 30px;\">продолжить</button></form></div>",
				PaymentResultStatus = PaymentResultStatus.Success
			};

			return paymentData;
		}

		public async Task<PivdenniyPaymentResponse> SendPaymentAsync(int orderId, int clientId, int amount, string cardToken, string purchaseTime, PaymentDescription description)
		{
			var descriptionString = GeneratePaymentDescription(description);

			var headerObj = new
			{
				alg = "RS256",
			};

			//COMISSION

			var payloadObj = new
			{
				MerchantID = _pivdenniyBankConfig.MerchantID,
				TerminalID = _pivdenniyBankConfig.TerminalID,
				OrderID = orderId.ToString(),
				UPCToken = cardToken,
				TotalAmount = amount,
				Currency = int.Parse(_pivdenniyBankConfig.Currency),
				PurchaseTime = purchaseTime,
				PurchaseDesc = descriptionString,
				Fee = (int)(amount * 0.025)
			};

			var headerString = JsonConvert.SerializeObject(headerObj);
			var payloadString = JsonConvert.SerializeObject(payloadObj);

			var header = EncryptionHelper.HashWithBASE64URL(headerString);
			var payload = EncryptionHelper.HashWithBASE64URL(payloadString);
			var signature = $"{header}.{payload}";
			var signatureBase64 = PemSignature.SignStringWithRS256(signature);

			HttpResponseMessage response;

			var paymentRequest = new PivdenniyPaymentRequestDto()
			{
				Header = header,
				Payload = payload,
				Signature = signatureBase64
			};

			try
			{
				var serializerSettings = new JsonSerializerSettings();
				serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
				var a = JsonConvert.SerializeObject(paymentRequest, serializerSettings);
				var paymentRequestJson = new StringContent(
					JsonConvert.SerializeObject(paymentRequest, serializerSettings),
					Encoding.UTF8,
					"text/plain");

				using (_httpClient)
				{
					response = await _httpClient.PostAsync("", paymentRequestJson);
				}
			}
			catch (Exception ex)
			{
				return null;
			}

			if (response.IsSuccessStatusCode)
			{
				var paymentResponse = JsonConvert.DeserializeObject<PivdenniyPaymentRequestDto>(await response.Content.ReadAsStringAsync());
				var paymentResponseDecoded = JsonConvert.DeserializeObject<PivdenniyPaymentResponse>(EncryptionHelper.DecodeBASE64URL(paymentResponse.Payload));

				return paymentResponseDecoded;
			}

			return null;
		}

		private string GeneratePaymentDescription(PaymentDescription description)
		{
			var result = $"{description.ClientFirstName}_{description.ClientLastName}_{(int)description.PaymentType}_{description.BillingId}_{description.BillingName}_{description.Month}_{description.Year}";
			return result;
		}

		public static Dictionary<string, string> ToDictionary(object obj)
		{
			var json = JsonConvert.SerializeObject(obj);
			var dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
			return dictionary;
		}
	}
}
