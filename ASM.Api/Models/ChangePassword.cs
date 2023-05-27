using System;

namespace ASM.Api.Models
{
    public class ChangePassword
    {
        public Guid SellerId { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
    }

    public class RecoveryPassword
    {
        public Guid SellerId { get; set; }
        public string Code { get; set; }
        public string NewPassword { get; set; }
    }
}
