namespace CisApi.Core.Application.Services;

using Xunit;
using Moq;
using CisApi.Core.Application.Services;
using CisApi.Core.Domain.Entities;
using CisApi.Core.Domain.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

public class TopicQueryServiceTests
{
    private readonly Mock<ITopicRepository> _mockRepository;
    private readonly TopicQueryService _service;

    public TopicQueryServiceTests()
    {
        _mockRepository = new Mock<ITopicRepository>();
        _service = new TopicQueryService(_mockRepository.Object);
    }

    [Fact]
    public async Task GetTopicsAsync_ShouldReturnAllTopics()
    {
        // Arrange
        var user = new User { Id = "1", Login = "testuser" };
        var topics = new List<Topic>
        {
            new Topic { Id = 1, Title = "Topic 1", Description = "Desc 1", CreatedBy = user, CreatedAt = DateTime.UtcNow },
            new Topic { Id = 2, Title = "Topic 2", Description = "Desc 2", CreatedBy = user, CreatedAt = DateTime.UtcNow }
        };
        _mockRepository.Setup(r => r.GetTopicsAsync()).ReturnsAsync(topics);

        // Act
        var result = await _service.GetTopicsAsync();

        // Assert
        Assert.Equal(topics.Count, result.ToList().Count);
        _mockRepository.Verify(r => r.GetTopicsAsync(), Times.Once);
    }

    [Fact]
    public async Task GetTopicAsync_ShouldReturnSingleTopic()
    {
        // Arrange
        var user = new User { Id = "1", Login = "testuser" };
        var topic = new Topic { Id = 1, Title = "Topic 1", Description = "Desc 1", CreatedBy = user, CreatedAt = DateTime.UtcNow };
        _mockRepository.Setup(r => r.GetTopicAsync(1)).ReturnsAsync(topic);

        // Act
        var result = await _service.GetTopicAsync(1);

        // Assert
        Assert.Equal(topic, result);
        _mockRepository.Verify(r => r.GetTopicAsync(1), Times.Once);
    }
}
