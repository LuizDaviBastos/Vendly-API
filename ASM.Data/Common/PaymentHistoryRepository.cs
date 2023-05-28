using ASM.Data.Entities;
using ASM.Data.Interfaces;

namespace ASM.Data.Common
{
    public class PaymentHistoryRepository : Repository<PaymentHistory>, IPaymentHistoryRepository
    {
        public PaymentHistoryRepository(AsmContext context): base(context) { }

    }
}
