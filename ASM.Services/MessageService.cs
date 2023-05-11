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

        public Task<IList<Attachment>> GetAttachments(Guid messageId)
        {
            throw new NotImplementedException();
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
