using ASM.Data.Entities;

namespace ASM.Api.Models
{
    public class PaymentInformationResult : PaymentInformation
    {
        public string ExpireInFormatted { get; set; }
        public string LastPaymentFormatted { get; set; }
        public double? Price { get; set; }
    }
}
