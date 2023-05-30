using System.Threading.Tasks;

namespace ASM.Data.Interfaces
{
    public interface IUnitOfWork
    {
        public ISellerRepository SellerRepository { get; }
        public IMeliAccountRepository MeliAccountRepository { get; }
        public IMessageRepository MessageRepository { get; }
        public ISellerOrderRepository SellerOrderRepository { get; }
        public IBillingInformationRepository BillingInformationRepository { get; }
        public IPaymentHistoryRepository PaymentHistoryRepository { get; }
        public ISellerFcmTokenRepository SellerFcmTokenRepository { get; }

        public void Commit();
        public Task CommitAsync();
    }
}
