namespace CisApi.Core.Domain.Entities;

public class Idea
{
    public int Id { get; set; }
    public int TopicId { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public required User CreatedBy { get; set; } = null!;
    public IEnumerable<string> VotedBy { get; set; } = new List<string>();
    public int VoteCount { get; set; }
}
