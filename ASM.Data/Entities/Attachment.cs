using System;
using System.Text.Json.Serialization;

namespace ASM.Data.Entities
{
    public class Attachment : IEntityBase
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
        public Guid MessageId { get; set; }
        public virtual SellerMessage Message { get; set; }
    }
}
