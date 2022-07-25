using ASM.Core.Interfaces;
using ASM.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASM.Data.Interfaces
{
    public interface ISellerRepository : IRepository<Seller>
    {
        public Task<Seller> UpdateMessage(string message, long sellerId);
        public Seller GetBySellerId(long sellerId);
        public Seller GetByAccessToken(string accessToken);
        public void DisableSeller(Seller seller);
        public void EnableSeller(Seller seller);
        public IEnumerable<Seller> GetActiveBillings();
    }
}
