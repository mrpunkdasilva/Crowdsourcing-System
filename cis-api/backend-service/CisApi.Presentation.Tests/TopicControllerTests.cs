using Xunit;
using Moq;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CisApi.Core.Domain.Entities;
using CisApi.Core.Domain.Services;
using CisApi.Presentation.Controllers;
using CisApi.Presentation.Dtos;
using Microsoft.AspNetCore.Http;

namespace CisApi.Presentation.Tests;

public class TopicControllerTests
{
    private readonly Mock<ITopicCommandService> _mockTopicCommandService;
    private readonly TopicController _controller;

    public TopicControllerTests()
    {
        _mockTopicCommandService = new Mock<ITopicCommandService>();
        _controller = new TopicController(_mockTopicCommandService.Object);
        
        // Simulate HttpContext.Items["User"] for the placeholder user
        var httpContext = new DefaultHttpContext();
        httpContext.Items["User"] = new User { Id = "1", Login = "testuser" };
        _controller.ControllerContext = new ControllerContext()
        {
            HttpContext = httpContext
        };
    }

    [Fact]
    public async Task CreateTopicAsync_ShouldReturn201Created_WhenTopicIsCreatedSuccessfully()
    {
        // Arrange
        var requestDto = new TopicRequestDto { Title = "New Topic", Description = "Description of new topic" };
        var createdTopic = new Topic
        {
            Id = 1,
            Title = requestDto.Title,
            Description = requestDto.Description,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = new User { Id = "1", Login = "testuser" }
        };

        _mockTopicCommandService.Setup(s => s.CreateTopicAsync(It.IsAny<User>(), It.IsAny<Topic>()))
                                .ReturnsAsync(createdTopic);

        // Act
        var result = await _controller.CreateTopicAsync(requestDto);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var topicResponseDto = Assert.IsType<TopicResponseDto>(createdAtActionResult.Value);

        Assert.Equal(201, createdAtActionResult.StatusCode);
        Assert.Equal(createdTopic.Id, topicResponseDto.Id);
        Assert.Equal(createdTopic.Title, topicResponseDto.Title);
        Assert.Equal(createdTopic.Description, topicResponseDto.Description);
        Assert.Equal(createdTopic.CreatedBy.Id, topicResponseDto.CreatedBy.Id);
        Assert.Equal(createdTopic.CreatedBy.Login, topicResponseDto.CreatedBy.Login);
        _mockTopicCommandService.Verify(s => s.CreateTopicAsync(It.IsAny<User>(), It.IsAny<Topic>()), Times.Once);
    }

    [Fact]
    public async Task CreateTopicAsync_ShouldReturnUnauthorized_WhenUserIsNotInHttpContextItems()
    {
        // Arrange
        var requestDto = new TopicRequestDto { Title = "New Topic", Description = "Description of new topic" };
        
        // Clear HttpContext.Items["User"] to simulate unauthorized scenario
        _controller.ControllerContext.HttpContext.Items.Remove("User");

        // Act
        var result = await _controller.CreateTopicAsync(requestDto);

        // Assert
        Assert.IsType<UnauthorizedObjectResult>(result.Result);
        _mockTopicCommandService.Verify(s => s.CreateTopicAsync(It.IsAny<User>(), It.IsAny<Topic>()), Times.Never);
    }

    [Fact]
    public async Task CreateTopicAsync_ShouldReturnBadRequest_WhenServiceThrowsInvalidOperationException()
    {
        // Arrange
        var requestDto = new TopicRequestDto { Title = "New Topic", Description = "Description of new topic" };
        _mockTopicCommandService.Setup(s => s.CreateTopicAsync(It.IsAny<User>(), It.IsAny<Topic>()))
                                .ThrowsAsync(new InvalidOperationException("User not found."));

        // Act
        var result = await _controller.CreateTopicAsync(requestDto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal(400, badRequestResult.StatusCode);
        Assert.Equal("User not found.", badRequestResult.Value);
        _mockTopicCommandService.Verify(s => s.CreateTopicAsync(It.IsAny<User>(), It.IsAny<Topic>()), Times.Once);
    }

    [Fact]
    public void GetTopicById_ShouldReturnNotFound()
    {
        // Arrange
        var id = 1;

        // Act
        var result = _controller.GetTopicById(id);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }
}