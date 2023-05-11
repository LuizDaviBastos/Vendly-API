using ASM.Services.Interfaces;
using ASM.Services.Models;
using Azure.Storage.Blobs;
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
                throw new Exception(ex.Message);
            }
            
        }

        public async Task Upload(Stream stream, Guid sellerId, string fileName)
        {
            BlobClient blobClient = new(configuration.AzureWebJobsStorage, "asmstorage", Path.Combine("attachments", sellerId.ToString(), fileName));
            var result = await blobClient.UploadAsync(stream);
        }

        public async Task<MemoryStream> Download(Guid sellerId, string fileName)
        {
            BlobClient blobClient = new(configuration.AzureWebJobsStorage, "asmstorage", Path.Combine("attachments", sellerId.ToString(), fileName));
            MemoryStream stream = new();
            await blobClient.DownloadToAsync(stream);
            return stream;
        }

        public async Task<List<MemoryStream>> Download(Guid sellerId, IEnumerable<string> fileNames)
        {
            List<MemoryStream> files = new List<MemoryStream>();
            foreach (var fileName in fileNames)
            {
                var ms = await Download(sellerId, fileName);
                files.Add(ms);
            }

            return files;
        }
    }
}