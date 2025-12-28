using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace CisApi.Infrastructure.MongoDB.Documents;

public class IdeaDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
#pragma warning disable CS8618 // Non-nullable property 'Id' must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public string Id { get; set; }
#pragma warning restore CS8618
    
    public required int MySqlId { get; set; }
    public required string TopicId { get; set; } // Adicionado
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required UserDocument CreatedBy { get; set; }
    public List<VoteDocument> Votes { get; set; } = new();
    public required int VoteCount { get; set; } // Modificado para leitura/escrita
}