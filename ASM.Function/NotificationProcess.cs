using ASM.Core.Models;
using ASM.Core.Repositories;
using ASM.Data.Entities;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace ASM.Core.Function
{
    public class NotificationProcess
    {
        private readonly IRepository<Seller> sellerRepository;
        public NotificationProcess(IRepository<Seller> sellerRepository)
        {
            this.sellerRepository = sellerRepository;
        }

        [FunctionName("NotificationProcess")]
        public void Run([QueueTrigger("process-order-notification")]NotificationTrigger notification, ILogger log)
        {
            /*
             TODO - 
                1 - verify if this is the first message
                2 - Get message by sellerId
                3 - Build string message 
                4 - Send message
                5 - check as first message
             */
        }
    }
}
