using ASM.Data.Entities;
using ASM.Data.Interfaces;
using System.Linq;

namespace ASM.Data.Common
{
    public class MeliAccountRepository : Repository<MeliAccount>, IMeliAccountRepository
    {
        public MeliAccountRepository(AsmContext context) : base(context) { }

        public MeliAccount? GetByMeliSellerId(long meliSellerId)
        {
            return dbSet.FirstOrDefault(x => x.MeliSellerId == meliSellerId);
        }
    }
}
