using ASM.Data.Entities;
using ASM.Data.Enums;
using ASM.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ASM.Data.Common
{
    public class SellerRepository : Repository<Seller>, ISellerRepository
    {
        public SellerRepository(AsmContext context): base(context) { }

        public void DisableSeller(Seller seller)
        {
            seller.BillingInformation.Status = BillingStatus.Inactive;
        }

        public void EnableSeller(Seller seller)
        {
            seller.BillingInformation.Status = BillingStatus.Active;
        }

        public IEnumerable<Seller> GetActiveBillings()
        {
            return dbSet.Where(x => x.BillingInformation.Status == BillingStatus.Active);
        }

        public Seller? GetByAccessToken(string accessToken)
        {
            return dbSet.FirstOrDefault(x => x.AccessToken == accessToken);
        }

        public Seller? GetByMeliSellerId(long sellerId)
        {
            return dbSet.FirstOrDefault(x => x.SellerId == sellerId);
        }

        public IQueryable<Seller> GetQueryable()
        {
            return dbSet.AsQueryable();
        }

        public SellerMessage? UpdateMessage(SellerMessage sellerMessage)
        {
            dbSet.Update(sellerMessage);
            return sellerMessage;
        }

        public override void AddOrUpdate(Seller entity)
        {
            
        }
    }
}
