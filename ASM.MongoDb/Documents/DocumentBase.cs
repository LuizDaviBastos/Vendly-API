using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASM.MongoDb.Documents
{
    public class DocumentBase : MongoDB.Bson.BsonDocument
    {
        [BsonId]
        public string? Id { get; set; }
    }
}
