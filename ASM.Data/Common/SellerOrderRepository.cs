using ASM.Data.Entities;
using ASM.Data.Interfaces;

namespace ASM.Data.Common
{
    public class SellerOrderRepository : Repository<SellerOrder>, ISellerOrderRepository
    {
        public SellerOrderRepository(AsmContext dbContext) : base(dbContext) { }
    }
}
