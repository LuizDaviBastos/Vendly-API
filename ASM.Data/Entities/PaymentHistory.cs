﻿using ASM.Data.Enums;
using System;
using System.Text.Json.Serialization;

namespace ASM.Data.Entities
{
    public class PaymentHistory : IEntityBase
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal Price { get; set; }
        public string? MetaData { get; set; }
        public Guid? UserPaymentId { get; set; }
        public PaymentStatus? Status { get; set; }
        public DateTime ExpireIn { get; set; }
        public string? PreferenceId { get; set; }


        public Guid SellerId { get; set; }

        [JsonIgnore]
        public virtual Seller Seller { get; set; }

        public Guid? SubscriptionPlanId { get; set; }
        public virtual SubscriptionPlan SubscriptionPlan { get; set; }
    }
}
