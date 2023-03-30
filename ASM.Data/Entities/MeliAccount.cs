using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASM.Data.Entities
{
    public class MeliAccount : EntityBase
    {
        public long MeliSellerId { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        public Guid? SellerId { get; set; }
        public virtual Seller Seller { get; set; }
    }
}
