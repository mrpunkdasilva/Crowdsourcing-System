using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CisApi.Infrastructure.MongoDb.Entities
{
    [BsonIgnoreExtraElements]
    public class TopicEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string MongoId { get; set; } = null!;

        [BsonElement("id")]
        public int id { get; set; } = 0;

        [BsonElement("Title")]
        public required string Title { get; set; }

        [BsonElement("Description")]
        public required string Description { get; set; }

        [BsonElement("Created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("CreatedBy")]
        public UserEntity CreatedBy { get; set; } = null!;

        [BsonIgnore]

        public List<IdeaEntity> Ideas { get; set; } = new List<IdeaEntity>();
    }
}
