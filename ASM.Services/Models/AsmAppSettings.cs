using MongoDB.Bson.Serialization.Attributes;

namespace ASM.Services.Models
{
    public class AsmAppSettings
    {
        [BsonId]
        [BsonElement("Id")]
        public string Id { get; set; }

        [BsonElement("Endpoint")]
        public string? Endpoint { get; set; }
    }
}
