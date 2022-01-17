using ASM.Core.Interfaces;
using ASM.Data.Entities;
using System.Threading.Tasks;

namespace ASM.Data.Interfaces
{
    public interface IPaymentInformationRepository : IRepository<PaymentInformation>
    {
        public Task DisableSellerAsync(PaymentInformation paymentInformation);
        public Task EnableSellerAsync(PaymentInformation paymentInformation);
    }
}
