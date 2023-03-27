using ASM.Core.Interfaces;
using ASM.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASM.Data.Interfaces
{
    public interface ISellerRepository : IRepository<Seller, Guid>
    {
        public SellerMessage? UpdateMessage(SellerMessage sellerMessage);
        public Seller? GetByMeliSellerId(long sellerId);
        public Seller? GetByAccessToken(string accessToken);
        public IQueryable<Seller> GetQueryable();
        public void DisableSeller(Seller seller);
        public void EnableSeller(Seller seller);
        public IEnumerable<Seller> GetActiveBillings();
    }
}
