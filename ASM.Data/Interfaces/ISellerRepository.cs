using ASM.Core.Interfaces;
using ASM.Data.Entities;
using System.Threading.Tasks;

namespace ASM.Data.Interfaces
{
    public interface ISellerRepository : IRepository<Seller>
    {
        public Task<Seller> UpdateMessage(string message, long sellerId);
    }
}
