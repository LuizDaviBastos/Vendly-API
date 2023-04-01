using System.Threading.Tasks;

namespace ASM.Data.Interfaces
{
    public interface IUnitOfWork
    {
        public ISellerRepository SellerRepository { get; }
        public IMeliAccountRepository MeliAccountRepository { get; }
        public IMessageRepository MessageRepository { get; }

        public void Commit();
        public Task CommitAsync();
    }
}
