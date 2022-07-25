using ASM.Data.Common;
using ASM.Data.Interfaces;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace ASM.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IMongoDatabase mongoDatabase;
        private ISellerRepository sellerRepository;

        public UnitOfWork(IMongoDatabase mongoDatabase)
        {
            this.mongoDatabase = mongoDatabase;
        }

        public ISellerRepository SellerRepository 
        { 
            get 
            {
                if (sellerRepository == null) sellerRepository = new MongoSellerRepository(mongoDatabase);
                return sellerRepository;
            }
            
        }

        public void Commit()
        {
            
        }

        public async Task CommitAsync()
        {
            
        }
    }
}
