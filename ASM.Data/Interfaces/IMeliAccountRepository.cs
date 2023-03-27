using ASM.Core.Interfaces;
using ASM.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASM.Data.Interfaces
{
    public interface IMeliAccountRepository : IRepository<MeliAccount, Guid>
    {
        public MeliAccount? GetByMeliSellerId(long meliSellerId);
    }
}
