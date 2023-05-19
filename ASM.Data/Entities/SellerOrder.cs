using ASM.Data.Enums;
using System;

namespace ASM.Data.Entities
{
    public class SellerOrder : IEntityBase
    {
        public Guid Id { get; set; }
        public long OrderId { get; set; }
        public bool? AfterSellerMessageStatus { get; set; }
        public bool? ShippingMessageStatus { get; set; }
        public bool? DeliveredMessageStatus { get; set; }

        public Guid SellerId { get; set; }
        public virtual Seller Seller { get; set; }
    }
}
