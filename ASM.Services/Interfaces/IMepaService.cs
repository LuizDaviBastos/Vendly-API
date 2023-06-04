using ASM.Services.Models.Mepa;
using MercadoPago.Resource;
using MercadoPago.Resource.Payment;

namespace ASM.Services.Interfaces
{
    public interface IMepaService
    {
        public Task<PaymentResponse> GetPaymentInformation(string paymentId);
        public Task<PaymentLinkResponse> CreatePreference(Guid sellerId, bool isBinary = false);
        public Task<PaymentLinkResponse> CreatePreferenceApi(Guid sellerId);
        public Task<PaymentLinkResponse> GetPreference(string preferenceId);
        public Task<ResultsResourcesPage<Payment>> GetLastPayments(Guid sellerId, int limit = 3);
    }
}
