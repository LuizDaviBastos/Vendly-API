using ASM.Data.Entities;
using System;

namespace ASM.Api.Models
{
    public class PaymentInformationResult
    {
        public PaymentInformationResult() { }

        public PaymentInformationResult(PaymentInformation pInfo, Guid sellerId)
        {
            ExpireIn = pInfo.ExpireIn.Value;
            LastPayment = pInfo.LastPayment;
            SellerId = sellerId;
            IsFreePeriod = pInfo.SubscriptionPlan.IsFree;
            SubscriptionPlan = pInfo.SubscriptionPlan;
            Price = pInfo.SubscriptionPlan.Price;
        }

        public DateTime? LastPayment { get; set; }
        public DateTime? ExpireIn { get; set; }
        public bool? IsFreePeriod { get; set; }
        public string? CurrentPlan { get; set; }
        public Guid SellerId { get; set; }
        public Guid SubscriptionPlanId { get; set; }
        public SubscriptionPlan SubscriptionPlan { get; set; }
        public decimal? Price { get; set; }
    }
}
