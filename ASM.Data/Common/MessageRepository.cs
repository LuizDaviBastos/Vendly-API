using ASM.Data.Entities;
using ASM.Data.Enums;
using ASM.Data.Interfaces;
using System.Linq;

namespace ASM.Data.Common
{
    public class MessageRepository : Repository<SellerMessage>, IMessageRepository
    {
        public MessageRepository(AsmContext context) : base(context) { }

        public SellerMessage? GetMessage(long meliSellerId, MessageType messageType)
        {
            return dbSet.Where(x => x.MeliAccount.MeliSellerId == meliSellerId && x.Type == messageType).FirstOrDefault();
        }
    }
}
