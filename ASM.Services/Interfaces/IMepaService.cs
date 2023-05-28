using ASM.Services.Models.Mepa;

namespace ASM.Services.Interfaces
{
    public interface IMepaService
    {
        public Task<PaymentResponse> GetPaymentInformation(string paymentId);
        public Task<PaymentLinkResponse> CreatePayment(Guid sellerId);
    }
}
