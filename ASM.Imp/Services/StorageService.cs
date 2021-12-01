using ASM.Core.Services;

namespace ASM.Imp.Services
{
    public class StorageService : IStorageService
    {
        public async Task SendMessageAsync(string queueName, object item)
        {
            //TODO Send to az storage queue
        }
    }
}