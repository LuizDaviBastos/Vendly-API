using ASM.Data.Entities;
using ASM.Data.Interfaces;

namespace ASM.Data.Common
{
    public class SubscriptionPlanRepository : Repository<SubscriptionPlan>, ISubscriptionPlanRepository
    {
        public SubscriptionPlanRepository(AsmContext context): base(context) { }

    }
}
