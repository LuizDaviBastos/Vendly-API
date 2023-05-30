using System;

namespace ASM.Api.Models
{
    public class RegisterFcmToken
    {
        public Guid SellerId { get; set; }
        public string? FcmToken { get; set; }
    }
}
