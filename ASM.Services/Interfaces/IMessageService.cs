using ASM.Data.Entities;
using ASM.Data.Enums;

namespace ASM.Services.Interfaces
{
    public interface IMessageService
    {
        public Task UpdateMessage(SellerMessage updateMessage);
        public Task<SellerMessage?> GetMessage(Guid meliAccountId, MessageType messageType);
        public Task<IList<Attachment>> GetAttachments(Guid messageId);
    }
}
