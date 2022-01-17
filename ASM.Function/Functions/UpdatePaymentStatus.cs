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
        private readonly IPaymentInformationRepository paymentInformationRepository;
        public UpdatePaymentStatus(IPaymentInformationRepository paymentInformationRepository)
        {
            this.paymentInformationRepository = paymentInformationRepository;
        }

        [FunctionName("UpdatePaymentStatus")]
        public async Task Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        {

            var activeBillingSellers = paymentInformationRepository.GetQueryableAsNoTracking(x => x.Status == StatusEnum.Active);

            foreach (var billing in activeBillingSellers)
            {
                if(billing.ExpireIn >= DateTime.UtcNow)
                {
                    await paymentInformationRepository.DisableSellerAsync(billing);
                }
            }

            log.LogInformation($"Payment status updated on: {DateTime.Now}");
        }
    }
}
