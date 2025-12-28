using Xunit;
using Moq;
using AutoMapper;
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
    /// Testes do VoteCommandController
    /// </summary>
    public class VoteCommandControllerTests
    {
        private readonly Mock<IVoteCommandService> _mockService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly VoteCommandController _controller;
        private readonly User _testUser;

        public VoteCommandControllerTests()
        {
            _mockService = new Mock<IVoteCommandService>();
            _mockMapper = new Mock<IMapper>();
            _controller = new VoteCommandController(_mockService.Object, _mockMapper.Object);
            _testUser = new User { Id = "1", Login = "dummy" };
            
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
        }

        // ========== TESTES: CreateVoteAsync ==========

        [Fact]
        public async Task CreateVoteAsync_ShouldReturn200WithIdea_WhenValid()
        {
            // ARRANGE
            var topicId = 1;
            var ideaId = 1;

            var votedIdea = new Idea
            {
                Id = ideaId,
                TopicId = topicId,
                Title = "Test Idea",
                Description = "Test Description",
                CreatedBy = _testUser,
                CreatedAt = DateTime.UtcNow,
                VoteCount = 1,
                VotedBy = new List<string> { "1" }
            };

            var responseDto = new IdeaResponseDto
            {
                Id = ideaId,
                TopicId = topicId,
                Title = "Test Idea",
                Description = "Test Description",
                VoteCount = 1,
                VotedBy = new List<string> { "1" },
                CreatedAt = DateTime.UtcNow,
                CreatedBy = new UserDto { Id = "1", Login = "dummy" }
            };

            _mockService
                .Setup(s => s.CreateVoteAsync(_testUser, ideaId))
                .ReturnsAsync(votedIdea);

            _mockMapper
                .Setup(m => m.Map<IdeaResponseDto>(votedIdea))
                .Returns(responseDto);

            _controller.ControllerContext.HttpContext.Items["User"] = _testUser;

            // ACT
            var result = await _controller.CreateVoteAsync(topicId, ideaId);

            // ASSERT
            var actionResult = result.Result;
            Assert.IsType<OkObjectResult>(actionResult);
            
            var okResult = actionResult as OkObjectResult;
            Assert.IsType<IdeaResponseDto>(okResult!.Value);
            
            var response = okResult.Value as IdeaResponseDto;
            Assert.Equal(ideaId, response!.Id);
            Assert.Equal(1, response.VoteCount);
            Assert.Single(response.VotedBy);
            
            _mockService.Verify(s => s.CreateVoteAsync(_testUser, ideaId), Times.Once);
            _mockMapper.Verify(m => m.Map<IdeaResponseDto>(votedIdea), Times.Once);
        }

        [Fact]
        public async Task CreateVoteAsync_ShouldReturnUnauthorized_WhenUserNotInContext()
        {
            // ARRANGE
            var topicId = 1;
            var ideaId = 1;

            // NÃO adicionar User no HttpContext

            // ACT
            var result = await _controller.CreateVoteAsync(topicId, ideaId);

            // ASSERT
            var actionResult = result.Result;
            Assert.IsType<UnauthorizedObjectResult>(actionResult);
            
            var unauthorizedResult = actionResult as UnauthorizedObjectResult;
            Assert.Equal("User information not found", unauthorizedResult!.Value);
        }

        // ========== TESTES: DeleteVoteAsync ==========

        [Fact]
        public async Task DeleteVoteAsync_ShouldReturn200WithIdea_WhenValid()
        {
            // ARRANGE
            var topicId = 1;
            var ideaId = 1;

            var unvotedIdea = new Idea
            {
                Id = ideaId,
                TopicId = topicId,
                Title = "Test Idea",
                Description = "Test Description",
                CreatedBy = _testUser,
                CreatedAt = DateTime.UtcNow,
                VoteCount = 0,
                VotedBy = new List<string>()
            };

            var responseDto = new IdeaResponseDto
            {
                Id = ideaId,
                TopicId = topicId,
                Title = "Test Idea",
                Description = "Test Description",
                VoteCount = 0,
                VotedBy = new List<string>(),
                CreatedAt = DateTime.UtcNow,
                CreatedBy = new UserDto { Id = "1", Login = "dummy" }
            };

            _mockService
                .Setup(s => s.DeleteVoteAsync(_testUser, ideaId))
                .ReturnsAsync(unvotedIdea);

            _mockMapper
                .Setup(m => m.Map<IdeaResponseDto>(unvotedIdea))
                .Returns(responseDto);

            _controller.ControllerContext.HttpContext.Items["User"] = _testUser;

            // ACT
            var result = await _controller.DeleteVoteAsync(topicId, ideaId);

            // ASSERT
            var actionResult = result.Result;
            Assert.IsType<OkObjectResult>(actionResult);
            
            var okResult = actionResult as OkObjectResult;
            var response = okResult!.Value as IdeaResponseDto;
            Assert.Equal(0, response!.VoteCount);
            Assert.Empty(response.VotedBy);
            
            _mockService.Verify(s => s.DeleteVoteAsync(_testUser, ideaId), Times.Once);
            _mockMapper.Verify(m => m.Map<IdeaResponseDto>(unvotedIdea), Times.Once);
        }

        [Fact]
        public async Task DeleteVoteAsync_ShouldReturnUnauthorized_WhenUserNotInContext()
        {
            // ARRANGE
            var topicId = 1;
            var ideaId = 1;

            // NÃO adicionar User no HttpContext

            // ACT
            var result = await _controller.DeleteVoteAsync(topicId, ideaId);

            // ASSERT
            var actionResult = result.Result;
            Assert.IsType<UnauthorizedObjectResult>(actionResult);
            
            var unauthorizedResult = actionResult as UnauthorizedObjectResult;
            Assert.Equal("User information not found", unauthorizedResult!.Value);
        }
    }
}
