using ASM.Data.Common;
using ASM.Data.Interfaces;
using System.Threading.Tasks;

namespace ASM.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AsmContext asmContext;
        private ISellerRepository sellerRepository;
        private IMeliAccountRepository meliAccountRepository;
        private IMessageRepository messageRepository;

        public UnitOfWork(AsmContext asmContext)
        {
            this.asmContext = asmContext;
        }

        public ISellerRepository SellerRepository 
        { 
            get 
            {
                if (sellerRepository == null) sellerRepository = new SellerRepository(asmContext);
                return sellerRepository;
            }
        }

        public IMeliAccountRepository MeliAccountRepository
        {
            get
            {
                if (meliAccountRepository == null) meliAccountRepository = new MeliAccountRepository(asmContext);
                return meliAccountRepository;
            }
        }

        public IMessageRepository MessageRepository
        {
            get
            {
                if (messageRepository == null) messageRepository = new MessageRepository(asmContext);
                return messageRepository;
            }
        }

        public void Commit()
        {
            asmContext.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await asmContext.SaveChangesAsync();
        }
    }
}
