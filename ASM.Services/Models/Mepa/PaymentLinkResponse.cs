﻿using MercadoPago.Resource.Preference;

namespace ASM.Services.Models.Mepa
{
    public class PaymentLinkResponse : RequestResponseBase
    {
        public string? preferenceId { get; set; }
        public decimal? price { get; set; }
        public string? init_point { get; set; }

    }
}
