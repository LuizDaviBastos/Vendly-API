using ASM.Data.Enums;
using System;

namespace ASM.Data.Entities
{
    public class SellerMessage : EntityBase
    {
        public string? Message { get; set; }
        public bool Activated { get; set; }
        public MessageType Type { get; set; }

        public Guid SellerId { get; set; }
        public virtual Seller Seller { get; set; }
        
    }
}
