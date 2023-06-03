using ASM.Services.Models.Mepa;
using MercadoPago.Resource.Preference;

namespace ASM.Services.Interfaces
{
    public interface IMepaService
    {
        public Task<PaymentResponse> GetPaymentInformation(string paymentId);
        public Task<PaymentLinkResponse> CreatePreference(Guid sellerId);
        public Task<PaymentLinkResponse> CreatePreferenceSdk(Guid sellerId);
        public Task<PaymentLinkResponse> GetPreference(string preferenceId);
    }
}
