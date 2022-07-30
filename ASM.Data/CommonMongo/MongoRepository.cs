using ASM.Core.Interfaces;
using ASM.Data.Documents;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;

namespace ASM.Data.Common
{
    public class MongoRepository<TDocument> : IRepository<TDocument> where TDocument : DocumentBase
    {
        protected readonly IMongoDatabase mongoDatabase;
        protected readonly IMongoCollection<TDocument> collection;
        public MongoRepository(IMongoDatabase mongoDatabase)
        {
            this.mongoDatabase = mongoDatabase;
            collection = mongoDatabase.GetCollection<TDocument>(typeof(TDocument).Name);
            try
            {
                var document = collection.Find(x => true).FirstOrDefault();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void Add(TDocument document)
        {
            collection.InsertOne(document);
        }

        public void Delete(string id)
        {
            collection.DeleteOne(x => x.id == id);
        }

        public TDocument Get(string id)
        {
            try
            {
                return collection.Find(x => x.id == id).FirstOrDefault();
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public void Update(TDocument entity)
        {
            try
            {
                collection.ReplaceOne(x => x.id == entity.id, entity);
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
