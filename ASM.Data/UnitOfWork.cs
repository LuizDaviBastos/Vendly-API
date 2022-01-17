using ASM.Data.Common;
using ASM.Data.Contexts;
using ASM.Data.Interfaces;
using System.Threading.Tasks;

namespace ASM.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AsmContext context;
        private ISellerRepository sellerRepository;
        private IPaymentInformationRepository paymentInformationRepository;

        public UnitOfWork(AsmContext context)
        {
            this.context = context;
        }

        public ISellerRepository SellerRepository 
        { 
            get 
            {
                if (sellerRepository == null) sellerRepository = new SellerRepository(context);
                return sellerRepository;
            }
            
        }

        public IPaymentInformationRepository PaymentInformationRepository 
        {
            get
            {
                if (paymentInformationRepository == null) paymentInformationRepository = new PaymentInformationRepository(context);
                return paymentInformationRepository;
            }
        }

        public void Commit()
        {
            context.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
