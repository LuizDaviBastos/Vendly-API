using MercadoPago.Resource.Preference;

namespace ASM.Services.Models.Mepa
{
    public class PaymentLinkResponse : RequestResponseBase
    {
        public string id { get; set; }
        public double? price { get; set; }
        public string? init_point { get; set; }
    }
}
