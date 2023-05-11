namespace ASM.Services.Interfaces
{
    public interface IStorageService
    {
        public Task SendMessageAsync(string queueName, object item);
        public Task Upload(Stream stream, Guid sellerId, string fileName);
        public Task<MemoryStream> Download(Guid sellerId, string fileName);
        public Task<List<MemoryStream>> Download(Guid sellerId, IEnumerable<string> fileNames);
    }
}
