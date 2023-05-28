using ASM.Core.Interfaces;
using ASM.Data.Entities;
using ASM.Data.Enums;
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
        public Task<Seller?> GetByEmailAsync(string email);
        public IEnumerable<Seller> GetActiveBillings();
        public Task<Seller?> MeliSellerExist(long meliSellerId);
    }
}
