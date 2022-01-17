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

        [FunctionName("UpdatePaymentStatus")]
        public async Task Run([TimerTrigger("%CronUpdateStatusSeller%")]TimerInfo myTimer, ILogger log)
        {
            var activeBillingSellers = uow.PaymentInformationRepository.GetQueryableAsNoTracking(x => x.Status == StatusEnum.Active);

            foreach (var billing in activeBillingSellers)
            {
                if(billing.ExpireIn >= DateTime.UtcNow)
                {
                    await uow.PaymentInformationRepository.DisableSellerAsync(billing);
                }
            }

            await uow.CommitAsync();

            log.LogInformation($"Payment status updated on: {DateTime.Now}");
        }
    }
}
