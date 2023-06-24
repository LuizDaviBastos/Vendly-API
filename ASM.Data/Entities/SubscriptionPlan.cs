using ASM.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ASM.Data.Entities
{
    public class SubscriptionPlan : IEntityBase
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public bool IsFree { get; set; }
        public int ValidateValue { get; set; }
        public ValidateType ValidateType { get; set; }
        public string? Description { get; set; }

        [JsonIgnore]
        public virtual IList<PaymentHistory> PaymentHistories { get; set; }

        [JsonIgnore]
        public virtual IList<PaymentInformation> PaymentInformations { get; set; }
        
    }
}
