using ASM.Core.Interfaces;
using ASM.Data.Entities;
using ASM.Data.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASM.Data.Interfaces
{
    public interface IMessageRepository : IRepository<SellerMessage, Guid>
    {
        public Task<SellerMessage?> GetMessage(Guid meliAccountId, MessageType messageType);
        public void AddAttachment(Attachment attachment);
        public Task<IList<Attachment>> GetAttachments(Guid messageId);
    }
}
