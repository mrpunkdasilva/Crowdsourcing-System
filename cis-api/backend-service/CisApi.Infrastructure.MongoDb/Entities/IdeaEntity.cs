using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CisApi.Infrastructure.MongoDb.Entities
{
    [BsonIgnoreExtraElements]
    public class IdeaEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string MongoId { get; set; } = null!;

        [BsonElement("id")]
        public int Id { get; set; } = 0;

        [BsonElement("topic_id")]
        public int TopicId { get; set; }

        [BsonIgnore]
        public required TopicEntity Topic { get; set; }

        [BsonElement("title")]
        public required string Title { get; set; }

        [BsonElement("description")]
        public required string Description { get; set; }

        [BsonElement("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("created_by")]
        public required UserEntity CreatedBy { get; set; }

        [BsonElement("vote_count")]
        public int VoteCount { get; set; }
        
        [BsonElement("VotedBy")]
        public ICollection<IdeaVotesEntity> VotedBy { get; set; } = new List<IdeaVotesEntity>();
    }
}