namespace CisApi.Infrastructure.MongoDB.Documents;

public class VoteDocument
{
    public required string IdeaId { get; set; } // Adicionado
    public required string UserId { get; set; }
    public required string UserLogin { get; set; }
    public required DateTime VotedAt { get; set; }
}