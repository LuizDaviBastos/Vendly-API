using ASM.Data.Entities;
using MongoDB.Driver;

namespace ASM.MongoDb.Common
{
    public class MongoPaymentInformationRepository : MongoRepository<PaymentInformation>, IPaymentInformationRepository
    {
        public MongoPaymentInformationRepository(IMongoDatabase mongoDatabase) : base(mongoDatabase) {}

       
    }
}
