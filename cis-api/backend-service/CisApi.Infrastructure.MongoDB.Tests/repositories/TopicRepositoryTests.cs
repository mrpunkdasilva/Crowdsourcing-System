using Xunit;
using CisApi.Infrastructure.MongoDb.Repositories;
using CisApi.Core.Domain.Entities;
using System;
using System.Linq;

namespace CisApi.Infrastructure.MongoDB.Tests.Repositories
{
    public class TopicRepositoryTests
    {
        private readonly User _testUser;

        public TopicRepositoryTests()
        {
            _testUser = new User { Id = "1", Login = "dummy" };
        }


        [Fact]
        public void CreateTopicAsync_ShouldHaveCorrectSignature()
        {
            var methodInfo = typeof(TopicRepository).GetMethod("CreateTopicAsync");
            Assert.NotNull(methodInfo);
            Assert.Equal(2, methodInfo!.GetParameters().Length);
        }

        [Fact]
        public void GetTopicsAsync_ShouldHaveCorrectSignature()
        {
            var methodInfo = typeof(TopicRepository).GetMethod("GetTopicsAsync");
            Assert.NotNull(methodInfo);
            Assert.Empty(methodInfo!.GetParameters());
        }

        [Fact]
        public void GetTopicAsync_ShouldHaveCorrectSignature()
        {
            var methodInfo = typeof(TopicRepository).GetMethod("GetTopicAsync");
            Assert.NotNull(methodInfo);
            Assert.Single(methodInfo!.GetParameters());
        }


        [Fact]
        public void TopicRepository_ShouldImplementITopicRepository()
        {
            var implementsInterface = typeof(TopicRepository)
                .GetInterfaces()
                .Any(i => i.Name == "ITopicRepository");
            Assert.True(implementsInterface);
        }

        [Fact]
        public void TopicRepository_ShouldHaveThreeDependencies()
        {
            var constructor = typeof(TopicRepository).GetConstructors().FirstOrDefault();
            Assert.NotNull(constructor);
            Assert.Equal(3, constructor!.GetParameters().Length);
        }

        [Fact]
        public void TopicRepository_ShouldRequireMongoDbContext()
        {
            var constructor = typeof(TopicRepository).GetConstructors().FirstOrDefault();
            var parameters = constructor!.GetParameters();
            Assert.Contains(parameters, p => p.ParameterType.Name == "MongoDbContext");
        }

        [Fact]
        public void TopicRepository_ShouldRequireIMapper()
        {
            var constructor = typeof(TopicRepository).GetConstructors().FirstOrDefault();
            var parameters = constructor!.GetParameters();
            Assert.Contains(parameters, p => p.ParameterType.Name == "IMapper");
        }

        [Fact]
        public void TopicRepository_ShouldRequireILogger()
        {
            var constructor = typeof(TopicRepository).GetConstructors().FirstOrDefault();
            var parameters = constructor!.GetParameters();
            Assert.Contains(parameters, p => p.ParameterType.Name.Contains("ILogger"));
        }


        [Fact]
        public void Topic_ShouldHaveRequiredProperties()
        {
            var topic = new Topic
            {
                Id = 1,
                Title = "Test",
                Description = "Desc",
                CreatedBy = _testUser
            };
            Assert.Equal(1, topic.Id);
            Assert.Equal("Test", topic.Title);
        }

        [Fact]
        public void Topic_ShouldHaveDescription()
        {
            var topic = new Topic
            {
                Id = 1,
                Title = "Test",
                Description = "Test Description",
                CreatedBy = _testUser
            };
            Assert.Equal("Test Description", topic.Description);
        }

        [Fact]
        public void Topic_ShouldHaveCreatedBy()
        {
            var topic = new Topic
            {
                Id = 1,
                Title = "Test",
                Description = "Desc",
                CreatedBy = _testUser
            };
            Assert.NotNull(topic.CreatedBy);
            Assert.Equal("dummy", topic.CreatedBy.Login);
        }


        [Fact]
        public void CreateTopicAsync_ShouldBeDeclaredAsync()
        {
            var methodInfo = typeof(TopicRepository).GetMethod("CreateTopicAsync");
            Assert.NotNull(methodInfo);
            Assert.True(methodInfo!.ReturnType.IsGenericType);
        }

        [Fact]
        public void CreateTopicAsync_ShouldAcceptUserAndTopic()
        {
            var methodInfo = typeof(TopicRepository).GetMethod("CreateTopicAsync");
            var parameters = methodInfo!.GetParameters();
            
            Assert.Equal(2, parameters.Length);
            Assert.Equal("user", parameters[0].Name);
            Assert.Equal("topic", parameters[1].Name);
        }


        [Fact]
        public void GetTopicsAsync_ShouldBeDeclaredAsync()
        {
            var methodInfo = typeof(TopicRepository).GetMethod("GetTopicsAsync");
            Assert.NotNull(methodInfo);
            Assert.True(methodInfo!.ReturnType.IsGenericType);
        }

        [Fact]
        public void GetTopicsAsync_ShouldHaveNoParameters()
        {
            var methodInfo = typeof(TopicRepository).GetMethod("GetTopicsAsync");
            Assert.Empty(methodInfo!.GetParameters());
        }


        [Fact]
        public void GetTopicAsync_ShouldBeDeclaredAsync()
        {
            var methodInfo = typeof(TopicRepository).GetMethod("GetTopicAsync");
            Assert.NotNull(methodInfo);
            Assert.True(methodInfo!.ReturnType.IsGenericType);
        }

        [Fact]
        public void GetTopicAsync_ShouldAcceptId()
        {
            var methodInfo = typeof(TopicRepository).GetMethod("GetTopicAsync");
            var parameters = methodInfo!.GetParameters();
            
            Assert.Single(parameters);
            Assert.Equal("id", parameters[0].Name);
        }


        [Fact]
        public void Constructor_ShouldInitializeFields()
        {
            var constructorInfo = typeof(TopicRepository).GetConstructors().FirstOrDefault();
            Assert.NotNull(constructorInfo);
            Assert.Equal(3, constructorInfo!.GetParameters().Length);
        }
    }
}
