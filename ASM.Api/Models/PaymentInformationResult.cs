using ASM.Data.Entities;

namespace ASM.Api.Models
{
    public class PaymentInformationResult : PaymentInformation
    {
        public double? Price { get; set; }
    }
}
