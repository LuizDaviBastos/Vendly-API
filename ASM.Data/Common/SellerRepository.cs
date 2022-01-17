using ASM.Data.Contexts;
using ASM.Data.Entities;
using ASM.Data.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace ASM.Data.Common
{
    public class SellerRepository : Repository<Seller>, ISellerRepository
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
                entity.Id = seller.Id;
                Update(entity);
            }
        }

        public async Task<Seller> UpdateMessage(string message, long sellerId)
        {
            var seller = GetQueryableAsNoTracking(x => x.SellerId == sellerId).FirstOrDefault();
            seller.Message = message;

            context.Entry(seller).Property(x => x.AccessToken).IsModified = false;
            context.Entry(seller).Property(x => x.RefreshToken).IsModified = false;
            context.Entry(seller).Property(x => x.SellerId).IsModified = false;
            context.Update(seller);

            await context.SaveChangesAsync();
            return seller;
        }
    }
}
