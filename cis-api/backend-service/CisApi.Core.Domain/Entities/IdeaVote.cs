namespace CisApi.Core.Domain.Entities;

public class IdeaVote
{
    public int IdeaId { get; set; }

    public required Idea Idea { get; set; }

    public required string UserId { get; set; }

    public required User User { get; set; }

    public DateTime VotedAt { get; set; }
}