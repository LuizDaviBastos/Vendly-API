using System.Threading.Tasks;

namespace ASM.Data.Interfaces
{
    public interface IUnitOfWork
    {
        public ISellerRepository SellerRepository { get; }

        public void Commit();
        public Task CommitAsync();
    }
}
