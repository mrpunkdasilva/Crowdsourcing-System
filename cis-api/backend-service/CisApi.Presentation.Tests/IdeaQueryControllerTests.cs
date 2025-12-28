using Moq;
using Microsoft.AspNetCore.Mvc;
using CisApi.Presentation.Controllers;
using CisApi.Presentation.Dtos;
using CisApi.Core.Domain.Entities;
using CisApi.Core.Domain.Services;


namespace CisApi.Presentation.Tests.Controllers
{
    public class IdeaQueryControllerTests
    {
        private readonly Mock<IIdeaQueryService> _mockService;
        private readonly IdeaQueryController _controller;
        private readonly User _testUser;

        public IdeaQueryControllerTests()
        {
            _mockService = new Mock<IIdeaQueryService>();
            _controller = new IdeaQueryController(_mockService.Object);
            _testUser = new User { Id = "1", Login = "dummy" };
        }

        [Fact]
        public async Task GetIdeasAsync_ShouldReturn200WithIdeas_WhenIdeasExist()
        {
            // ARRANGE
            var topicId = 1;
            
            var ideas = new List<Idea>
            {
                new Idea
                {
                    Id = 1,
                    TopicId = topicId,
                    Title = "Idea 1",
                    Description = "Description 1",
                    CreatedBy = _testUser,
                    CreatedAt = DateTime.UtcNow,
                    VoteCount = 5,
                    VotedBy = new List<string> { "1", "2", "3" }
                },
                new Idea
                {
                    Id = 2,
                    TopicId = topicId,
                    Title = "Idea 2",
                    Description = "Description 2",
                    CreatedBy = _testUser,
                    CreatedAt = DateTime.UtcNow,
                    VoteCount = 3,
                    VotedBy = new List<string> { "1", "2" }
                }
            };

            _mockService
                .Setup(s => s.GetIdeasByTopicIdAsync(topicId))
                .Returns(Task.FromResult<IEnumerable<Idea>>(ideas));

            // ACT
            var result = await _controller.GetIdeasAsync(topicId);

            // ASSERT
            var actionResult = result.Result;
            Assert.IsType<OkObjectResult>(actionResult);
            
            var okResult = actionResult as OkObjectResult;
            Assert.IsType<IdeaResponseDto[]>(okResult!.Value);
            
            var responseArray = okResult.Value as IdeaResponseDto[];
            Assert.Equal(2, responseArray!.Length);
            Assert.Equal("Idea 1", responseArray[0].Title);
            Assert.Equal("Idea 2", responseArray[1].Title);
            Assert.Equal(5, responseArray[0].VoteCount);
            Assert.Equal(3, responseArray[1].VoteCount);
            
            _mockService.Verify(s => s.GetIdeasByTopicIdAsync(topicId), Times.Once);
        }

        [Fact]
        public async Task GetIdeasAsync_ShouldReturn200WithEmptyArray_WhenNoIdeasExist()
        {
            // ARRANGE
            var topicId = 1;
            var emptyList = new List<Idea>();

            _mockService
                .Setup(s => s.GetIdeasByTopicIdAsync(topicId))
                .Returns(Task.FromResult<IEnumerable<Idea>>(emptyList));

            // ACT
            var result = await _controller.GetIdeasAsync(topicId);

            // ASSERT
            var actionResult = result.Result;
            Assert.IsType<OkObjectResult>(actionResult);
            
            var okResult = actionResult as OkObjectResult;
            var responseArray = okResult!.Value as IdeaResponseDto[];
            Assert.NotNull(responseArray);
            Assert.Empty(responseArray);
        }

        [Fact]
        public async Task GetIdeasAsync_ShouldMapAllPropertiesCorrectly()
        {
            // ARRANGE
            var topicId = 1;
            
            var ideas = new List<Idea>
            {
                new Idea
                {
                    Id = 10,
                    TopicId = topicId,
                    Title = "Test Idea",
                    Description = "Test Description",
                    CreatedBy = _testUser,
                    CreatedAt = new DateTime(2025, 10, 30, 8, 0, 0, DateTimeKind.Utc),
                    VoteCount = 7,
                    VotedBy = new List<string> { "1", "2", "3", "4" }
                }
            };

            _mockService
                .Setup(s => s.GetIdeasByTopicIdAsync(topicId))
                .Returns(Task.FromResult<IEnumerable<Idea>>(ideas));

            // ACT
            var result = await _controller.GetIdeasAsync(topicId);

            // ASSERT
            var okResult = result.Result as OkObjectResult;
            var responseArray = okResult!.Value as IdeaResponseDto[];
            var response = responseArray![0];
            
            Assert.Equal(10, response.Id);
            Assert.Equal(topicId, response.TopicId);
            Assert.Equal("Test Idea", response.Title);
            Assert.Equal("Test Description", response.Description);
            Assert.Equal(7, response.VoteCount);
            Assert.NotNull(response.CreatedBy);
            Assert.Equal("1", response.CreatedBy.Id);
            Assert.Equal("dummy", response.CreatedBy.Login);
        }
    }
}
