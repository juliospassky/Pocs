using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities.Common
{
    public abstract class EntityBase
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; }
    }
}
