using ASM.Data.Entities;
using ASM.Data.Enums;
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

    }
}
