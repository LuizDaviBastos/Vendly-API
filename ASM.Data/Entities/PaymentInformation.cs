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
        public bool? IsFreePeriod { get; set; }
        public string? CurrentPlan { get; set; }

        public static PaymentInformation GetFreePeriod()
        {
            return new PaymentInformation
            {
                CurrentPlan = "15 Dias",
                Status = BillingStatus.Active,
                IsFreePeriod = true,
                ExpireIn = DateTime.UtcNow.AddDays(15)
            };
        }
    }
}
