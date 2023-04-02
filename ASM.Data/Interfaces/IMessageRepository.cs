using ASM.Core.Interfaces;
using ASM.Data.Entities;
using ASM.Data.Enums;
using System;

namespace ASM.Data.Interfaces
{
    public interface IMessageRepository : IRepository<SellerMessage, Guid>
    {
        public SellerMessage? GetMessage(Guid meliAccountId, MessageType messageType);
    }
}
