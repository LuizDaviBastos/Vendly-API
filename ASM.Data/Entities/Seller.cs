using ASM.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASM.Data.Entities
{
    public class Seller : Entity
    {
        public long SellerId { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string? Message { get; set; }


        public Seller() { }

        public Seller(AccessToken accessToken)
        {
            AccessToken = accessToken.access_token;
            SellerId = accessToken.user_id;
            RefreshToken = accessToken.refresh_token;
        }
    }
}
