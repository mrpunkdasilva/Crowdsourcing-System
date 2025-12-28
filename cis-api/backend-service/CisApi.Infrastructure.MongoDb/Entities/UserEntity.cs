using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CisApi.Infrastructure.MongoDb.Entities
{
    [BsonIgnoreExtraElements]
    public class UserEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public required string MongoId { get; set; }

        [BsonElement("id")]
        public string id { get; set; } = "";

        [BsonElement("login")]
        public required string login { get; set; }

        [BsonIgnore]
        public ICollection<TopicEntity> TopicsCreated { get; set; } = new List<TopicEntity>();
        [BsonIgnore]
        public ICollection<IdeaEntity> IdeasCreated { get; set; } = new List<IdeaEntity>();
        [BsonIgnore]
        public ICollection<IdeaVotesEntity> Votes { get; set; } = new List<IdeaVotesEntity>();
    }
}
