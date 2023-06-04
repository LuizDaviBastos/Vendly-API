using ASM.Data.Enums;
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
        public long? PaymentId { get; set; }

        public Guid SellerId { get; set; }

        [JsonIgnore]
        public virtual Seller Seller { get; set; }
    }
}
