using MongoDB.Driver;

namespace ASM.MongoDb
{
    public class ASMMongoClient
    {
        private readonly MongoClient mongoClient;
        public ASMMongoClient(MongoClient mongoClient)
        {
            this.mongoClient = mongoClient;
        }
    }
}