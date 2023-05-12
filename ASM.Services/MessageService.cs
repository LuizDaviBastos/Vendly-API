using ASM.Data.Entities;
using ASM.Data.Enums;
using ASM.Data.Interfaces;
using ASM.Services.Interfaces;

namespace ASM.Services
{
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork uow;

        public MessageService(IUnitOfWork unitOfWork)
        {
            this.uow = unitOfWork;
        }

        public async Task AddAttachment(Attachment attachment)
        {
            uow.MessageRepository.AddAttachment(attachment);
            await uow.CommitAsync();
        }

        public async Task<Attachment> DeleteAttachment(Guid attachmentId)
        {
            var attachment = uow.MessageRepository.DeleteAttachment(attachmentId);
            await uow.CommitAsync();
            return attachment;
        }

        public async Task<IList<Attachment>> GetAttachments(Guid messageId)
        {
            return await uow.MessageRepository.GetAttachments(messageId);
        }

        public async Task<SellerMessage?> GetMessage(Guid meliAccountId, MessageType messageType)
        {
            var message = await uow.MessageRepository.GetMessage(meliAccountId, messageType);
            return message;
        }

        public async Task UpdateMessage(SellerMessage updateMessage)
        {
            uow.MessageRepository.AddOrUpdate(updateMessage);
            await uow.CommitAsync();
        }
    }
}
