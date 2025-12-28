namespace CisApi.Core.Domain.Entities;

public class Topic
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public User CreatedBy { get; set; } = null!;
    public List<Idea> Ideas { get; set; } = new();
}
