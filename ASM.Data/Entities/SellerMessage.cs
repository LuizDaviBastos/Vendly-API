using ASM.Data.Enums;
using System;

namespace ASM.Data.Entities
{
    public class SellerMessage : IEntityBase
    {
        public Guid Id { get; set; }
        public string? Message { get; set; }
        public bool Activated { get; set; }
        public MessageType Type { get; set; }

        public Guid? MeliAccountId { get; set; }
        public virtual MeliAccount MeliAccount { get; set; }
    }
}
