using System.Threading.Tasks;
using CottageApi.Core.Domain.Dto.Billings;
using CottageApi.Core.Domain.Entities;

namespace CottageApi.Core.Services
{
	public interface IBankPivdenniyService
	{
		Task CreatePaymentResponseAsync(PivdenniyPaymentResponse response);

		PaymentDataDto GetCardAddingData(int orderId, int clientId, PaymentDescription description);

		Task<PivdenniyPaymentResponse> SendPaymentAsync(int orderId, int clientId, int amount, string cardToken, string purchaseTime, PaymentDescription description);

		PaymentDataDto GetPaymentData(int orderId, int clientId, int amount, PaymentDescription description);
	}
}
