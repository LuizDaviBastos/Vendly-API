using ASM.Data.Enums;

namespace ASM.Services.Interfaces
{
    public interface IStorageService
    {
        public Task SendMessageAsync(string queueName, object item);
        public Task<string> Upload(Stream stream, Guid sellerId, MessageType messageType, string fileName);
        public Task<MemoryStream> Download(Guid sellerId, MessageType messageType, string fileName);
        public Task<List<MemoryStream>> Download(Guid sellerId, MessageType messageType, IEnumerable<string> fileNames);
        public Task<bool> Delete(Guid sellerId, MessageType messageType, string fileName);
    }
}
