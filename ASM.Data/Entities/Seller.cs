using ASM.Data.Documents;

namespace ASM.Data.Entities
{
    public class Seller : DocumentBase
    {
        public long SellerId { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string? Message { get; set; }
        public bool? AfterSellerMessageEnabled { get; set; }
        public PaymentInformation BillingInformation { get; set; }
    }
}
