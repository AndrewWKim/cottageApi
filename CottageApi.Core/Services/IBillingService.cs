using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CottageApi.Core.Domain.Dto.Billings;
using CottageApi.Core.Domain.Entities;
using CottageApi.Core.Enums;

namespace CottageApi.Core.Services
{
	public interface IBillingService
	{
		Task<IEnumerable<CottageBilling>> GetCottageBillingsAsync(int userId);

		Task<IEnumerable<CottageBilling>> GetCottageBillingsForMeterAsync(int userId);

		Task<IEnumerable<CommunalBill>> GetCommunalBillsAsync(int cottageBillingId);

		Task<Tuple<IEnumerable<CommunalBill>, int>> GetCommunalBillsForAdminAsync(CommunalBillFilter filter);

		Task<int> GetUnpaidCommunalBillsCountAsync(int userId);

		/*Task<PaymentLinkDataDto> GenerateCommunalBillPaymentLinkAsync(int clientId, int communalBillId, int? cardId);

		Task<PaymentLinkDataDto> GenerateCottageBillingPaymentLinkAsync(int clientId, int cottageBillingId, int? cardId);*/

		Task<IEnumerable<CommunalBill>> GetPayedCommunalBillsAsync(DateTime from, DateTime to);

		Task CreateCottageBillingsAsync(IEnumerable<CreateCottageBillingDto> cottageBillingsDto);

		Task ChangePaymentStatusAsync(PaymentType paymentType, int paymentId, PaymentStatus paymentStatus);

		/*Task<PaymentResultAction> GetAndRequestForPaymentResultAsync(string orderId);*/

		Task<PaymentDataDto> GetCardAddingHTMLAsync(PaymentType paymentType, int clientId);

		Task ProcessPivdenniyPaymentResponseAsync(PivdenniyPaymentResponse response);

		Task StartCountdownForIfNoReposnseSetUnpaidAsync(int orderId);

		Task<PaymentDataDto> MakePaymentWithTokenAsync(PaymentType paymentType, int clientId, int cardId, int? billingId);

		Task<PaymentDataDto> GetPaymentHTMLAsync(PaymentType paymentType, int clientId, int? billingId);
	}
}
