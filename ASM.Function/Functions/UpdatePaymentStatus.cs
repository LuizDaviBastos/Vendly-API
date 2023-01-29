using ASM.Data.Enums;
using ASM.Data.Interfaces;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ASM.Function.Functions
{
    public class UpdatePaymentStatus
    {
        private readonly IUnitOfWork uow;
        public UpdatePaymentStatus(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        //Remove comment to work
        //[FunctionName("UpdatePaymentStatus")]
        public async Task Run([TimerTrigger("%CronUpdatePaymentStatus%")]TimerInfo myTimer, ILogger log)
        {
            var activeBillingSellers = uow.SellerRepository.GetActiveBillings();

            foreach (var billing in activeBillingSellers)
            {
                if(billing.BillingInformation.ExpireIn >= DateTime.UtcNow)
                {
                    uow.SellerRepository.DisableSeller(billing);
                }
            }

            await uow.CommitAsync();

            log.LogInformation($"Payment status updated on: {DateTime.Now}");
        }
    }
}
