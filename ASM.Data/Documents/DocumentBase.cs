using MongoDB.Bson.Serialization.Attributes;

namespace ASM.Data.Documents
{
    public class DocumentBase
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? id { get; set; }
    }
}
