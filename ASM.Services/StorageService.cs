using ASM.Data.Enums;
using ASM.Services.Interfaces;
using ASM.Services.Models;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
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

        public async Task<string> Upload(Stream stream, Guid sellerId, MessageType messageType, string fileName)
        {
            BlobContainerClient containerClient = new(configuration.AzureWebJobsStorage, "asmstorage");
            await containerClient.CreateIfNotExistsAsync();
            var blobClient = containerClient.GetBlobClient(GetBlobNameBySellerId(sellerId, messageType, fileName));
            if(await blobClient.ExistsAsync())
            {
                int count = 1;
                string baseFileName = Path.GetFileNameWithoutExtension(fileName);
                string fileExtension = Path.GetExtension(fileName);
                string newFileName = $"{baseFileName}({count}){fileExtension}";
                while (await containerClient.GetBlobClient(GetBlobNameBySellerId(sellerId, messageType, newFileName)).ExistsAsync())
                {
                    count++;
                    newFileName = $"{baseFileName}({count}){fileExtension}";
                }
                fileName = newFileName;
            }

            await containerClient.UploadBlobAsync(GetBlobNameBySellerId(sellerId, messageType, fileName), stream);
            return fileName;
        }

        public async Task<MemoryStream> Download(Guid sellerId, MessageType messageType, string fileName)
        {
            BlobClient blobClient = new(configuration.AzureWebJobsStorage, "asmstorage", GetBlobNameBySellerId(sellerId, messageType, fileName));
            MemoryStream stream = new();
            await blobClient.DownloadToAsync(stream);
            return stream;
        }

        public async Task<List<MemoryStream>> Download(Guid sellerId, MessageType messageType, IEnumerable<string> fileNames)
        {
            List<MemoryStream> files = new List<MemoryStream>();
            foreach (var fileName in fileNames)
            {
                var ms = await Download(sellerId, messageType, fileName);
                files.Add(ms);
            }

            return files;
        }

        public async Task<bool> Delete(Guid sellerId, MessageType messageType, string fileName)
        {
            BlobClient blobClient = new(configuration.AzureWebJobsStorage, "asmstorage", GetBlobNameBySellerId(sellerId, messageType, fileName));
            return await blobClient.DeleteIfExistsAsync();
        }

        private string GetBlobNameBySellerId(Guid sellerId, MessageType messageType, string fileName)
        {
            return Path.Combine("attachments", sellerId.ToString(), messageType.ToString(), fileName);
        }
    }
}