using ASM.Data.Entities;
using ASM.Data.Enums;
using ASM.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASM.Data.Common
{
    public class MessageRepository : Repository<SellerMessage>, IMessageRepository
    {
        public MessageRepository(AsmContext context) : base(context) { }

        public async Task<SellerMessage?> GetMessage(Guid meliAccountId, MessageType messageType)
        {
            var message = await dbSet.Where(x => x.MeliAccount.Id == meliAccountId && x.Type == messageType).FirstOrDefaultAsync();
            if (message == null)
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

        public void AddAttachment(Attachment attachment)
        {
            dbContext.Set<Attachment>().Add(attachment);
        }

        public async Task<IList<Attachment>> GetAttachments(Guid messageId)
        {
            var message = await GetQueryable().Where(x => x.Id == messageId)
            .Include(x => x.Attachments)
            .Select(x => new SellerMessage
            {
                Id = x.Id,
                Attachments = x.Attachments
            }).FirstOrDefaultAsync();

            return message?.Attachments ?? new List<Attachment>();
        }

        public Attachment DeleteAttachment(Guid attachmentId)
        {
            var attachment = dbContext.Set<Attachment>().Include(x => x.Message).FirstOrDefault(x => x.Id == attachmentId);
            if(attachment != null)
            {
                dbContext.Set<Attachment>().Remove(attachment);
            }
            return attachment;
        }
    }
}
