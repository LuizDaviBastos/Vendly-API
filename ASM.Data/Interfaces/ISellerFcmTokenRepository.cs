using ASM.Core.Interfaces;
using ASM.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASM.Data.Interfaces
{
    public interface ISellerFcmTokenRepository : IRepository<SellerFcmToken, Guid>
    {
        public Task<List<string>> GetTokens(Guid sellerId);
    }
}
