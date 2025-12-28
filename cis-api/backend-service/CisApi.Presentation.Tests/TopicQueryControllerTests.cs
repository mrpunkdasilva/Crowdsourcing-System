using Xunit;
using Moq;
using CisApi.Presentation.Controllers;
using CisApi.Core.Domain.Services;
using AutoMapper;
using System.Threading.Tasks;
using System.Collections.Generic;
using CisApi.Core.Domain.Entities;
using CisApi.Presentation.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CisApi.Presentation.Tests
{
    /// <summary>
    /// Contains unit tests for the <see cref="TopicQueryController"/> class.
    /// </summary>
    public class TopicQueryControllerTests
    {
        [Fact]
        public async Task GetTopicsAsync_ShouldReturnOkResult_WithListOfTopics()
        {
            // Arrange
            var mockService = new Mock<ITopicQueryService>();
            var mockMapper = new Mock<IMapper>();
            var controller = new TopicQueryController(mockService.Object, mockMapper.Object);

            var fakeUser = new User { Id = "user1", Login = "testuser" };
            var fakeUserDto = new UserDto { Id = "user1", Login = "testuser" };

            var fakeTopicsFromService = new List<Topic> 
            {
                new Topic { Id = 1, Title = "Topic 1", Description = "Desc 1", CreatedBy = fakeUser },
                new Topic { Id = 2, Title = "Topic 2", Description = "Desc 2", CreatedBy = fakeUser }
            };
            var fakeTopicDtosFromMapper = new List<TopicResponseDto> 
            {
                new TopicResponseDto { Id = 1, Title = "Topic 1", Description = "Desc 1", CreatedBy = fakeUserDto },
                new TopicResponseDto { Id = 2, Title = "Topic 2", Description = "Desc 2", CreatedBy = fakeUserDto }
            };

            mockService.Setup(s => s.GetTopicsAsync()).ReturnsAsync(fakeTopicsFromService);
            mockMapper.Setup(m => m.Map<IEnumerable<TopicResponseDto>>(fakeTopicsFromService)).Returns(fakeTopicDtosFromMapper);

            // Act
            var result = await controller.GetTopicsAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedDtos = Assert.IsAssignableFrom<IEnumerable<TopicResponseDto>>(okResult.Value);
            Assert.Equal(2, returnedDtos.Count());
            mockService.Verify(s => s.GetTopicsAsync(), Times.Once);
        }
    }
}
