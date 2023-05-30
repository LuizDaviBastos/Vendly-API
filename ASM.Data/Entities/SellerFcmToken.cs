using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace ASM.Data.Entities
{
    public class SellerFcmToken : IEntityBase
    {
        public Guid Id { get; set; }
        public string? FcmToken { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid SellerId { get; set; }
        public virtual Seller Seller { get; set; }
    }
}
