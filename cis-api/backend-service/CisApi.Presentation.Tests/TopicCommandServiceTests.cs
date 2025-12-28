namespace CisApi.Core.Application.Services;

using Xunit;
using Moq;
using CisApi.Core.Application.Services;
using CisApi.Core.Domain.Entities;
using CisApi.Core.Domain.Repositories;
using System.Threading.Tasks;

public class TopicCommandServiceTests
{
    private readonly Mock<ITopicRepository> _mockRepository;
    private readonly TopicCommandService _service;

    public TopicCommandServiceTests()
    {
        _mockRepository = new Mock<ITopicRepository>();
        _service = new TopicCommandService(_mockRepository.Object);
    }

    [Fact]
    public async Task CreateTopicAsync_ShouldCallRepository()
    {
        // Arrange
        var user = new User { Id = "1", Login = "testuser" };
        var topic = new Topic { Id = 1, Title = "Test Topic", Description = "Test Description", CreatedBy = user, CreatedAt = DateTime.UtcNow };
        _mockRepository.Setup(r => r.CreateTopicAsync(It.IsAny<User>(), It.IsAny<Topic>())).ReturnsAsync(topic);

        // Act
        var result = await _service.CreateTopicAsync(user, topic);

        // Assert
        Assert.Equal(topic, result);
        _mockRepository.Verify(r => r.CreateTopicAsync(It.IsAny<User>(), It.IsAny<Topic>()), Times.Once);
    }

    [Fact]
    public async Task CreateTopicAsync_ShouldThrowArgumentNullException_WhenTopicIsNull()
    {
        // Arrange
        Topic nullTopic = null!;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _service.CreateTopicAsync(new User { Id = "1", Login = "testuser" }, nullTopic));
        _mockRepository.Verify(r => r.CreateTopicAsync(It.IsAny<User>(), It.IsAny<Topic>()), Times.Never);
    }

    [Fact]
    public async Task CreateTopicAsync_ShouldThrowArgumentNullException_WhenUserIsNull()
    {
        // Arrange
        User nullUser = null!;
        var topic = new Topic { Id = 1, Title = "Test Topic", Description = "Test Description", CreatedBy = new User { Id = "1", Login = "testuser" }, CreatedAt = DateTime.UtcNow };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _service.CreateTopicAsync(nullUser, topic));
        _mockRepository.Verify(r => r.CreateTopicAsync(It.IsAny<User>(), It.IsAny<Topic>()), Times.Never);
    }
}
