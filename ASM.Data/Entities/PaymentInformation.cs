using ASM.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ASM.Data.Entities
{
    public class PaymentInformation : Entity
    {
        public StatusEnum? Status { get; set; }
        public DateTime? ExpireIn { get; set; }
        public virtual long SellerId { get; set; }
        public virtual Seller Seller { get; set; }
    }
}
