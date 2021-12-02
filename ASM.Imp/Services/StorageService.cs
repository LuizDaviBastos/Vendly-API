using ASM.Core.Services;
using ASM.Imp.Models;
using Azure.Storage.Queues;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;

namespace ASM.Imp.Services
{
    public class StorageService : IStorageService
    {
        private readonly AsmConfiguration configuration;
        public StorageService(AsmConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task SendMessageAsync(string queueName, object item)
        {
            try
            {
                QueueClient queueClient = new QueueClient(configuration.AzureStorageConnection, queueName);
                var message = JsonConvert.SerializeObject(item);
                //await queueClient.SendMessageAsync(new BinaryData(Encoding.UTF8.GetBytes(message)));
                var data = new BinaryData(Encoding.UTF8.GetBytes(message));
                await queueClient.SendMessageAsync(message);
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }
    }
}