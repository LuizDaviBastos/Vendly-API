using ASM.Data.Entities;
using ASM.Data.Enums;
using ASM.Data.Interfaces;
using System;
using System.Linq;

namespace ASM.Data.Common
{
    public class MessageRepository : Repository<SellerMessage>, IMessageRepository
    {
        public MessageRepository(AsmContext context) : base(context) { }

        public SellerMessage? GetMessage(Guid meliAccountId, MessageType messageType)
        {
            var message = dbSet.Where(x => x.MeliAccount.Id == meliAccountId && x.Type == messageType).FirstOrDefault();
            if(message == null)
            {
                message = new()
                {
                    MeliAccountId = meliAccountId,
                    Activated = false,
                    Type = messageType
                };
            }

            return message;
        }

        public override void AddOrUpdate(SellerMessage entity)
        {
            if (entity.Id == Guid.Empty) Add(entity);

            else Update(entity);
        }
    }
}
