using ASM.Services.Interfaces;
using ASM.Services.Models;
using Azure.Storage.Queues;
using Newtonsoft.Json;
using System.Text;

namespace ASM.Services
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
                QueueClient queueClient = new QueueClient(configuration.AzureWebJobsStorage, queueName);
                await queueClient.CreateIfNotExistsAsync();
                var json = JsonConvert.SerializeObject(item);
                var bytes = Encoding.UTF8.GetBytes(json);
                await queueClient.SendMessageAsync(Convert.ToBase64String(bytes));
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }
    }
}