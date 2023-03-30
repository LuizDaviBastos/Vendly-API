using ASM.Data.Enums;
using System;

namespace ASM.Data.Entities
{
    public class PaymentInformation : EntityBase
    {
        public BillingStatus? Status { get; set; }
        public DateTime? ExpireIn { get; set; }

        public Guid? SellerId { get; set; }
        public virtual Seller Seller { get; set; }
    }
}
