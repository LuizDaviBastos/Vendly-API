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
        private ISellerOrderRepository sellerOrderRepository;
        private IBillingInformationRepository billingInformationRepository;
        private IPaymentHistoryRepository paymentHistoryRepository;
        private ISellerFcmTokenRepository sellerFcmTokenRepository;
        private ISubscriptionPlanRepository subscriptionPlanRepository;

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

        public ISellerOrderRepository SellerOrderRepository
        {
            get
            {
                if (sellerOrderRepository == null) sellerOrderRepository = new SellerOrderRepository(asmContext);
                return sellerOrderRepository;
            }
        }

        public IBillingInformationRepository BillingInformationRepository 
        {
            get
            {
                if (billingInformationRepository == null) billingInformationRepository = new BillingInformationRepository(asmContext);
                return billingInformationRepository;
            }
        }

        public IPaymentHistoryRepository PaymentHistoryRepository
        {
            get
            {
                if (paymentHistoryRepository == null) paymentHistoryRepository = new PaymentHistoryRepository(asmContext);
                return paymentHistoryRepository;
            }
        }

        public ISellerFcmTokenRepository SellerFcmTokenRepository 
        {
            get
            {
                if (sellerFcmTokenRepository == null) sellerFcmTokenRepository = new SellerFcmTokenRepository(asmContext);
                return sellerFcmTokenRepository;
            }
        }

        public ISubscriptionPlanRepository SubscriptionPlanRepository
        {
            get
            {
                if (subscriptionPlanRepository == null) subscriptionPlanRepository = new SubscriptionPlanRepository(asmContext);
                return subscriptionPlanRepository;
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
