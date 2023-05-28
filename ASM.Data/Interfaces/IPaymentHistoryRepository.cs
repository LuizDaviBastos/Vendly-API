using ASM.Core.Interfaces;
using ASM.Data.Entities;
using System;

namespace ASM.Data.Interfaces
{
    public interface IPaymentHistoryRepository : IRepository<PaymentHistory, Guid>
    {
     
    }
}
