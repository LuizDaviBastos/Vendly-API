using ASM.Data.Entities;
using ASM.Data.Enums;
using ASM.Data.Interfaces;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASM.Data.Common
{
    public class MongoSellerRepository : MongoRepository<Seller>, ISellerRepository
    {
        public MongoSellerRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase) { }

        public override void AddOrUpdate(Seller entity)
        {
            var seller = GetBySellerId(entity.SellerId);
            if(seller == null)
            {
                Add(entity);
            }
            else
            {
                seller.AccessToken = entity.AccessToken;
                seller.RefreshToken = entity.RefreshToken;
                Update(seller);
            }
        }

        public void DisableSeller(Seller seller)
        {
            var result = Get(seller?.id);
            result.BillingInformation.Status = StatusEnum.Inactive;
            Update(result);
        }

        public void EnableSeller(Seller seller)
        {
            var result = Get(seller?.id);
            result.BillingInformation.Status = StatusEnum.Active;
            Update(result);
        }

        public IEnumerable<Seller> GetActiveBillings()
        {
            return collection.Find(x => x.BillingInformation.Status == StatusEnum.Active).ToList();
        }

        public Seller GetByAccessToken(string accessToken)
        {
            return collection.Find(x => x.AccessToken.ToLower() == accessToken.ToLower()).FirstOrDefault();
        }

        public Seller GetBySellerId(long sellerId)
        {
            return collection.Find(x => x.SellerId == sellerId).FirstOrDefault();
        }

        public async Task<Seller> UpdateMessage(string message, long sellerId)
        {
            var seller = GetBySellerId(sellerId);
            if (seller == null) return null;

            seller.Message = message;
            Update(seller);

            return seller;
        }
    }
}
