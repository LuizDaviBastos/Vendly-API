using ASM.Data.Entities;
using ASM.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASM.Data.Common
{
    public class SellerFcmTokenRepository : Repository<SellerFcmToken>, ISellerFcmTokenRepository
    {
        public SellerFcmTokenRepository(AsmContext context): base(context) { }

        public async Task<List<string>> GetTokens(Guid sellerId)
        {
            var result = await dbSet.Where(x => x.SellerId == sellerId).Select(x => x.FcmToken).ToListAsync();
            if (result?.Any() ?? false)
            {
                return result.Where(x => !string.IsNullOrEmpty(x)).ToList() ?? new();
            }

            return new();
        }

    }
}
