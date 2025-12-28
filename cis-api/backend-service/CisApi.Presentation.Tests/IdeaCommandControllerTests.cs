using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using CisApi.Presentation.Controllers;
using CisApi.Presentation.Dtos;
using CisApi.Core.Domain.Entities;
using CisApi.Core.Domain.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CisApi.Presentation.Tests.Controllers
{
    /// <summary>
    /// Testes do IdeaCommandController
    /// Framework: xUnit + Moq + Coverlet (SEM FluentAssertions)
    /// </summary>
    public class IdeaCommandControllerTests
    {
        private readonly Mock<IIdeaCommandService> _mockService;
        private readonly IdeaCommandController _controller;
        private readonly User _testUser;

        public IdeaCommandControllerTests()
        {
            _mockService = new Mock<IIdeaCommandService>();
            _controller = new IdeaCommandController(_mockService.Object);
            _testUser = new User { Id = "1", Login = "dummy" };
            
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
        }


        [Fact]
        public async Task CreateIdeaAsync_ShouldReturn201Created_WhenValid()
        {
            // ARRANGE
            var topicId = 1;
            var request = new IdeaRequestDto
            {
                Title = "Ideia Teste",
                Description = "Descrição do teste"
            };

            var createdIdea = new Idea
            {
                Id = 1,
                TopicId = topicId,
                Title = request.Title,
                Description = request.Description,
                CreatedBy = _testUser,
                CreatedAt = DateTime.UtcNow,
                VoteCount = 0,
                VotedBy = new List<string>()
            };

            _mockService
                .Setup(s => s.CreateIdeaForTopicAsync(topicId, request.Title, request.Description, _testUser))
                .ReturnsAsync(createdIdea);

            _controller.ControllerContext.HttpContext.Items["User"] = _testUser;

            // ACT
            var result = await _controller.CreateIdeaAsync(topicId, request);

            // ASSERT (SEM FluentAssertions)
            var actionResult = result.Result;
            Assert.IsType<CreatedAtActionResult>(actionResult);
            
            var createdResult = actionResult as CreatedAtActionResult;
            Assert.Equal(201, createdResult!.StatusCode);
            Assert.Equal(nameof(_controller.GetIdeaById), createdResult.ActionName);
            Assert.IsType<IdeaResponseDto>(createdResult.Value);
            
            var responseDto = createdResult.Value as IdeaResponseDto;
            Assert.Equal(1, responseDto!.Id);
            Assert.Equal("Ideia Teste", responseDto.Title);
            
            _mockService.Verify(
                s => s.CreateIdeaForTopicAsync(topicId, request.Title, request.Description, _testUser),
                Times.Once
            );
        }

        [Fact]
        public async Task CreateIdeaAsync_ShouldReturnBadRequest_WhenModelStateInvalid()
        {
            // ARRANGE
            var topicId = 1;
            var request = new IdeaRequestDto
            {
                Title = "",
                Description = ""
            };

            _controller.ModelState.AddModelError("Title", "Title is required");

            // ACT
            var result = await _controller.CreateIdeaAsync(topicId, request);

            // ASSERT (SEM FluentAssertions)
            var actionResult = result.Result;
            Assert.IsType<BadRequestObjectResult>(actionResult);
        }

        [Fact]
        public async Task CreateIdeaAsync_ShouldReturnUnauthorized_WhenUserNotInContext()
        {
            // ARRANGE
            var topicId = 1;
            var request = new IdeaRequestDto
            {
                Title = "Teste",
                Description = "Desc"
            };


            // ACT
            var result = await _controller.CreateIdeaAsync(topicId, request);

            // ASSERT (SEM FluentAssertions)
            var actionResult = result.Result;
            Assert.IsType<UnauthorizedResult>(actionResult);
        }

        [Fact]
        public async Task CreateIdeaAsync_ShouldReturnNotFound_WhenTopicNotFound()
        {
            // ARRANGE
            var topicId = 999;
            var request = new IdeaRequestDto
            {
                Title = "Teste",
                Description = "Desc"
            };

            var exception = new InvalidOperationException("Topic with ID 999 not found");
            
            _mockService
                .Setup(s => s.CreateIdeaForTopicAsync(topicId, request.Title, request.Description, _testUser))
                .ThrowsAsync(exception);

            _controller.ControllerContext.HttpContext.Items["User"] = _testUser;

            // ACT
            var result = await _controller.CreateIdeaAsync(topicId, request);

            // ASSERT (SEM FluentAssertions)
            var actionResult = result.Result;
            Assert.IsType<NotFoundObjectResult>(actionResult);
        }

        [Fact]
        public async Task CreateIdeaAsync_ShouldReturnUnauthorized_WhenUserNotFoundInService()
        {
            // ARRANGE
            var topicId = 1;
            var request = new IdeaRequestDto
            {
                Title = "Teste",
                Description = "Desc"
            };

            var exception = new InvalidOperationException("User with ID not found");
            
            _mockService
                .Setup(s => s.CreateIdeaForTopicAsync(topicId, request.Title, request.Description, _testUser))
                .ThrowsAsync(exception);

            _controller.ControllerContext.HttpContext.Items["User"] = _testUser;

            // ACT
            var result = await _controller.CreateIdeaAsync(topicId, request);

            // ASSERT
            var actionResult = result.Result;
            Assert.IsType<UnauthorizedObjectResult>(actionResult);
        }


        [Fact]
        public async Task GetIdeaById_ShouldReturn200Ok_WhenIdeaExists()
        {
            // ARRANGE
            var topicId = 1;
            var ideaId = 1;

            var idea = new Idea
            {
                Id = ideaId,
                TopicId = topicId,
                Title = "Ideia 1",
                Description = "Descrição",
                CreatedBy = _testUser,
                CreatedAt = DateTime.UtcNow,
                VoteCount = 2,
                VotedBy = new List<string> { "1", "2" }
            };

            _mockService
                .Setup(s => s.GetIdeaByIdAsync(topicId, ideaId))
                .ReturnsAsync(idea);

            // ACT
            var result = await _controller.GetIdeaById(topicId, ideaId);

            // ASSERT (SEM FluentAssertions)
            var actionResult = result.Result;
            Assert.IsType<OkObjectResult>(actionResult);
            
            var okResult = actionResult as OkObjectResult;
            Assert.Equal(200, okResult!.StatusCode);
            Assert.IsType<IdeaResponseDto>(okResult.Value);

            var responseDto = okResult.Value as IdeaResponseDto;
            Assert.Equal(ideaId, responseDto!.Id);
            Assert.Equal("Ideia 1", responseDto.Title);
            Assert.Equal(2, responseDto.VoteCount);
        }

        [Fact]
        public async Task GetIdeaById_ShouldReturn404NotFound_WhenIdeaNotExists()
        {
            // ARRANGE
            var topicId = 1;
            var ideaId = 999;

            var exception = new InvalidOperationException("Idea with ID 999 not found");

            _mockService
                .Setup(s => s.GetIdeaByIdAsync(topicId, ideaId))
                .ThrowsAsync(exception);

            // ACT
            var result = await _controller.GetIdeaById(topicId, ideaId);

            // ASSERT (SEM FluentAssertions)
            var actionResult = result.Result;
            Assert.IsType<NotFoundObjectResult>(actionResult);
        }

        [Fact]
        public async Task GetIdeaById_ShouldThrowException_WhenServiceThrowsUnexpectedError()
        {
            // ARRANGE
            var topicId = 1;
            var ideaId = 1;

            var exception = new InvalidOperationException("Unexpected database error");

            _mockService
                .Setup(s => s.GetIdeaByIdAsync(topicId, ideaId))
                .ThrowsAsync(exception);

            // ACT & ASSERT
            await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await _controller.GetIdeaById(topicId, ideaId)
            );
        }
    }
}
