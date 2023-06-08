using ASM.Data.Enums;
using System;
using System.Text.Json.Serialization;

namespace ASM.Data.Entities
{
    public class PaymentInformation : IEntityBase
    {
        public Guid Id { get; set; }
        public BillingStatus? Status { get; set; }
        public DateTime? LastPayment { get; set; }
        public DateTime? ExpireIn { get; set; }

        public Guid SellerId { get; set; }
        [JsonIgnore]
        public virtual Seller Seller { get; set; }

        public Guid? SubscriptionPlanId { get; set; }
        public virtual SubscriptionPlan SubscriptionPlan { get; set; }
    }
}
