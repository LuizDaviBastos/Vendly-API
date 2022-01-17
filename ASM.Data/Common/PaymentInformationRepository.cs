using ASM.Data.Contexts;
using ASM.Data.Entities;
using ASM.Data.Enums;
using ASM.Data.Interfaces;
using System;
using System.Threading.Tasks;

namespace ASM.Data.Common
{
    public class PaymentInformationRepository : Repository<PaymentInformation>, IPaymentInformationRepository
    {
        public PaymentInformationRepository(AsmContext context) : base(context) {}

        public async Task DisableSellerAsync(PaymentInformation paymentInformation)
        {
            paymentInformation.Status = StatusEnum.Inactive;

            context.Entry(paymentInformation).Property(x => x.ExpireIn).IsModified = false;
            context.Update(paymentInformation);

            Update(paymentInformation);
            await context.SaveChangesAsync();
        }

        public async Task EnableSellerAsync(PaymentInformation paymentInformation)
        {
            paymentInformation.Status = StatusEnum.Active;
            paymentInformation.ExpireIn = DateTime.UtcNow;
            Update(paymentInformation);
            await context.SaveChangesAsync();
        }
    }
}
