using ASM.Data.Entities;
using ASM.Data.Interfaces;

namespace ASM.Data.Common
{
    public class BillingInformationRepository : Repository<PaymentInformation>, IBillingInformationRepository
    {
        public BillingInformationRepository(AsmContext context): base(context) { }

    }
}
