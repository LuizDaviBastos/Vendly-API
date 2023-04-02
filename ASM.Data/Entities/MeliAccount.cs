using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ASM.Data.Entities
{
    public class MeliAccount : IEntityBase
    {
        public Guid Id { get; set; }
        public long MeliSellerId { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        public Guid? SellerId { get; set; }

        [JsonIgnore]
        public virtual Seller Seller { get; set; }

        public virtual IList<SellerMessage> Messages { get; set; }
    }
}
