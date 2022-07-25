using ASM.Data.Contexts;
using ASM.Data.Entities;
using ASM.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ASM.MongoDb.Common
{
    public class SellerRepository : MongoRepository<Seller>, ISellerRepository
    {
        public SellerRepository(AsmContext context) : base(context) { }

        public override void AddOrUpdate(Seller entity)
        {
            var seller = GetQueryableAsNoTracking(x => x.SellerId == entity.SellerId).FirstOrDefault();
            if(seller == null)
            {
                Add(entity);
            }
            else
            {
                seller.AccessToken = entity.AccessToken;
                seller.RefreshToken= entity.RefreshToken;
                Update(seller);
            }
        }

        public Seller GetByAccessToken(string accessToken)
        {
            return context.Sellers.Where(x => x.AccessToken.ToLower() == accessToken.ToLower()).FirstOrDefault();
        }

        public Seller GetBySellerId(long sellerId)
        {
            return context.Sellers.Where(x => x.SellerId == sellerId).FirstOrDefault();
        }

        public async Task<Seller> UpdateMessage(string message, long sellerId)
        {
            var seller = GetQueryableAsNoTracking(x => x.SellerId == sellerId).FirstOrDefault();
            if (seller == null) return null;
            seller.Message = message;

            context.Entry(seller).Property(x => x.AccessToken).IsModified = false;
            context.Entry(seller).Property(x => x.RefreshToken).IsModified = false;
            context.Entry(seller).Property(x => x.SellerId).IsModified = false;
            context.Update(seller);
            context.SaveChanges();

            return seller;
        }
    }
}
