using ASM.Core.Interfaces;
using ASM.MongoDb.Documents;
using MongoDB.Driver;

namespace ASM.MongoDb.Common
{
    public class MongoRepository<TDocument> : IRepository<TDocument> where TDocument : DocumentBase
    {
        protected readonly IMongoDatabase mongoDatabase;
        protected readonly IMongoCollection<TDocument> collection;
        public MongoRepository(IMongoDatabase mongoDatabase)
        {
            this.mongoDatabase = mongoDatabase;
            collection = mongoDatabase.GetCollection<TDocument>(nameof(TDocument));
        }

        public void Add(TDocument document)
        {
            collection.InsertOne(document);
        }

        public void Delete(string id)
        {
            collection.DeleteOne(x => x.Id == id);
        }

        public TDocument Get(string id)
        {
            return collection.Find(x => x.Id == id).FirstOrDefault();
        }

        public void Update(TDocument entity)
        {
            try
            {
                collection.UpdateOne(x => x.Id == entity.Id, entity);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public virtual void AddOrUpdate(TDocument entity)
        {
            throw new NotImplementedException();
        }
    }
}
