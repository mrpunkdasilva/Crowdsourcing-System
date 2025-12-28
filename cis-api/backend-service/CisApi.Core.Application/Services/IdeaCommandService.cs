using System;
using System.Threading.Tasks;
using CisApi.Core.Domain.Entities;
using CisApi.Core.Domain.Repositories;
using CisApi.Core.Domain.Services;

namespace CisApi.Core.Application.Services;
public class IdeaCommandService : IIdeaCommandService
{
    private readonly IIdeaRepository _repository;

    public IdeaCommandService(IIdeaRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<Idea> CreateIdeaAsync(User user, Idea idea)
    {
        return await _repository.CreateIdeaAsync(user, idea);
    }
    
    public async Task<Idea> GetIdeaByIdAsync(int topicId, int ideaId)
    {
        var idea = await _repository.GetByIdAsync(ideaId);


        if (idea == null)
            throw new InvalidOperationException($"Idea {ideaId} not found.");

        if (idea.TopicId != topicId)
            throw new InvalidOperationException($"Idea {ideaId} does not belong to Topic {topicId}.");

        return idea;
    }

    public async Task<Idea> CreateIdeaForTopicAsync(int topicId, string title, string description, User user)
    {
        var idea = new Idea
        {
            TopicId = topicId,
            Title = title,
            Description = description,
            CreatedBy = user,
            CreatedAt = DateTime.UtcNow
        };

        return await _repository.CreateIdeaAsync(user, idea);
    }
}