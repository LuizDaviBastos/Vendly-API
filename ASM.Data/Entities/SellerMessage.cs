using ASM.Data.Enums;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ASM.Data.Entities
{
    public class SellerMessage : IEntityBase
    {
        public Guid Id { get; set; }
        public string? Message { get; set; }
        public bool Activated { get; set; }
        public MessageType Type { get; set; }

        public Guid? MeliAccountId { get; set; }

        [JsonIgnore]
        public virtual MeliAccount MeliAccount { get; set; }

        [JsonIgnore]
        public IList<Attachment> Attachments { get; set; }
    }
}
