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
            return dbSet.FirstOrDefault(x => x.MeliAccounts.Any(x => x.AccessToken == accessToken));
        }

        public Seller? GetByMeliSellerId(long meliSellerId)
        {
            return dbSet.FirstOrDefault(x => x.MeliAccounts.Any(x => x.MeliSellerId == meliSellerId));
        }

        public IQueryable<Seller> GetQueryable()
        {
            return dbSet.AsQueryable();
        }

        public SellerMessage? UpdateMessage(SellerMessage sellerMessage)
        {
            var entity = dbContext.Set<SellerMessage>().FirstOrDefault(x => x.Id == sellerMessage.Id);
            if(entity != null)
            {
                entity.Message = sellerMessage.Message;
                entity.Activated = sellerMessage.Activated;
                dbContext.Set<SellerMessage>().Update(entity);
            }
            
            return sellerMessage;
        }

        public override void AddOrUpdate(Seller entity)
        {
            
        }
    }
}
