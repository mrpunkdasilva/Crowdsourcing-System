using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CisApi.Infrastructure.MongoDb.Entities
{
    [BsonIgnoreExtraElements]
    public class IdeaVotesEntity
    {
        //[BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        //public string MongoId { get; set; } = null!;

        //[BsonElement("id")]
        //public int id { get; set; } = 0;

        //[BsonElement("idea")]
        //public required IdeaEntity Idea { get; set; }

        [BsonElement("votedBy")]
        public required UserEntity User { get; set; }

        [BsonElement("voted_at")]
        public DateTime VotedAt { get; set; }  = DateTime.UtcNow;
    }
}