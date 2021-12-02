using ASM.Core.Repositories;
using ASM.Data.Contexts;
using ASM.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ASM.Data.Common
{
    public class SellerRepository : Repository<Seller>
    {
        public SellerRepository(AsmContext context) : base(context) { }

        public override void AddOrUpdate(Seller entity)
        {
            var seller = GetQueryable(x => x.SellerId == entity.SellerId).FirstOrDefault();
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
    }
}
