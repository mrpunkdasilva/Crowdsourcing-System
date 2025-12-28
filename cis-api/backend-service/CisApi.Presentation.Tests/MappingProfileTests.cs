using AutoMapper;
using CisApi.Core.Domain.Entities;
using CisApi.Presentation.Dtos;
using CisApi.Presentation.MappingProfiles;
using System;
using System.Collections.Generic;
using Xunit;

namespace CisApi.Presentation.Tests.Mappings
{
    /// <summary>
    /// Contains unit tests for the <see cref="MappingProfile"/> class.
    /// </summary>
    public class MappingProfileTests
    {
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configuration;

        public MappingProfileTests()
        {
            _configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = _configuration.CreateMapper();
        }

        [Fact]
        public void MappingProfile_ShouldHaveValidConfiguration()
        {
            // Assert
            _configuration.AssertConfigurationIsValid();
        }

        [Fact]
        public void Should_Map_Topic_To_TopicResponseDto()
        {
            // Arrange
            var topic = new Topic
            {
                Id = 1,
                Title = "Test Topic",
                Description = "Test Description",
                CreatedAt = new DateTime(2022, 1, 3, 10, 7, 19, DateTimeKind.Utc),
                CreatedBy = new User
                {
                    Id = "1",
                    Login = "testuser"
                }
            };

            // Act
            var dto = _mapper.Map<TopicResponseDto>(topic);

            // Assert
            Assert.NotNull(dto);
            Assert.Equal(topic.Id, dto.Id);
            Assert.Equal(topic.Title, dto.Title);
            Assert.Equal(topic.Description, dto.Description);
            Assert.Equal(topic.CreatedAt, dto.CreatedAt);
            Assert.NotNull(dto.CreatedBy);
            Assert.Equal(topic.CreatedBy.Id, dto.CreatedBy.Id);
            Assert.Equal(topic.CreatedBy.Login, dto.CreatedBy.Login);
        }

        [Fact]
        public void Should_Map_User_To_UserDto()
        {
            // Arrange
            var user = new User
            {
                Id = "1",
                Login = "testuser"
            };

            // Act
            var dto = _mapper.Map<UserDto>(user);

            // Assert
            Assert.NotNull(dto);
            Assert.Equal(user.Id, dto.Id);
            Assert.Equal(user.Login, dto.Login);
        }

        [Fact]
        public void Should_Map_Topic_WithNullCreatedBy_Correctly()
        {
            // Arrange
            var topic = new Topic
            {
                Id = 2,
                Title = "Topic Without User",
                Description = "Description",
                CreatedAt = DateTime.UtcNow,
                CreatedBy = null!
            };

            // Act & Assert
            // AutoMapper deve lidar com null sem lançar exceção
            var exception = Record.Exception(() => _mapper.Map<TopicResponseDto>(topic));
            Assert.Null(exception);
        }

        [Fact]
        public void Should_Map_Collection_Of_Topics_To_TopicResponseDtos()
        {
            // Arrange
            var topics = new List<Topic>
            {
                new Topic
                {
                    Id = 1,
                    Title = "Topic 1",
                    Description = "Description 1",
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = new User { Id = "1", Login = "user1" }
                },
                new Topic
                {
                    Id = 2,
                    Title = "Topic 2",
                    Description = "Description 2",
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = new User { Id = "2", Login = "user2" }
                }
            };

            // Act
            var dtos = _mapper.Map<IEnumerable<TopicResponseDto>>(topics);

            // Assert
            Assert.NotNull(dtos);
            var dtoList = Assert.IsAssignableFrom<IEnumerable<TopicResponseDto>>(dtos);
            Assert.Equal(2, ((List<TopicResponseDto>)dtoList).Count);
        }

        [Fact]
        public void Should_Map_User_WithEmptyStrings_Correctly()
        {
            // Arrange
            var user = new User
            {
                Id = "",
                Login = ""
            };

            // Act
            var dto = _mapper.Map<UserDto>(user);

            // Assert
            Assert.NotNull(dto);
            Assert.Equal(string.Empty, dto.Id);
            Assert.Equal(string.Empty, dto.Login);
        }

        [Fact]
        public void Should_Map_Topic_PreservingDateTimeKind()
        {
            // Arrange
            var utcDate = new DateTime(2022, 1, 3, 10, 7, 19, DateTimeKind.Utc);
            var topic = new Topic
            {
                Id = 5,
                Title = "DateTime Test",
                Description = "Testing DateTime preservation",
                CreatedAt = utcDate,
                CreatedBy = new User { Id = "1", Login = "test" }
            };

            // Act
            var dto = _mapper.Map<TopicResponseDto>(topic);

            // Assert
            Assert.Equal(utcDate, dto.CreatedAt);
            Assert.Equal(DateTimeKind.Utc, dto.CreatedAt.Kind);
        }

        [Fact]
        public void Should_Map_Multiple_Users_Independently()
        {
            // Arrange
            var user1 = new User { Id = "1", Login = "userone" };
            var user2 = new User { Id = "2", Login = "usertwo" };

            // Act
            var dto1 = _mapper.Map<UserDto>(user1);
            var dto2 = _mapper.Map<UserDto>(user2);

            // Assert
            Assert.NotEqual(dto1.Id, dto2.Id);
            Assert.NotEqual(dto1.Login, dto2.Login);
            Assert.Equal("userone", dto1.Login);
            Assert.Equal("usertwo", dto2.Login);
        }

        [Fact]
        public void Should_Map_Topic_WithAllProperties_Correctly()
        {
            // Arrange
            var topic = new Topic
            {
                Id = 99999,
                Title = "some name",
                Description = "some description",
                CreatedAt = new DateTime(2022, 1, 3, 10, 7, 19, 90, DateTimeKind.Utc),
                CreatedBy = new User
                {
                    Id = "1",
                    Login = "testuser"
                }
            };

            // Act
            var dto = _mapper.Map<TopicResponseDto>(topic);

            // Assert - Validar resposta conforme critério de aceitação
            Assert.Equal(99999, dto.Id);
            Assert.Equal("some name", dto.Title);
            Assert.Equal("some description", dto.Description);
            Assert.Equal(new DateTime(2022, 1, 3, 10, 7, 19, 90, DateTimeKind.Utc), dto.CreatedAt);
            Assert.NotNull(dto.CreatedBy);
            Assert.Equal("1", dto.CreatedBy.Id);
            Assert.Equal("testuser", dto.CreatedBy.Login);
        }
    }
}
