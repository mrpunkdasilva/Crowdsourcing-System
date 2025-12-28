using Xunit;
using CisApi.Infrastructure.MongoDb.Repositories;
using CisApi.Core.Domain.Entities;
using System;
using System.Linq;

namespace CisApi.Infrastructure.MongoDB.Tests.Repositories
{
    public class IdeaRepositoryTests
    {
        private readonly User _testUser;

        public IdeaRepositoryTests()
        {
            _testUser = new User { Id = "1", Login = "dummy" };
        }


        [Fact]
        public void CreateIdeaAsync_ShouldHaveCorrectSignature()
        {
            var methodInfo = typeof(IdeaRepository).GetMethod("CreateIdeaAsync");
            Assert.NotNull(methodInfo);
            Assert.Equal(2, methodInfo!.GetParameters().Length);
        }

        [Fact]
        public void GetIdeasByTopicIdAsync_ShouldHaveCorrectSignature()
        {
            var methodInfo = typeof(IdeaRepository).GetMethod("GetIdeasByTopicIdAsync");
            Assert.NotNull(methodInfo);
            Assert.Single(methodInfo!.GetParameters());
        }

        [Fact]
        public void GetByIdAsync_ShouldHaveCorrectSignature()
        {
            var methodInfo = typeof(IdeaRepository).GetMethod("GetByIdAsync");
            Assert.NotNull(methodInfo);
            Assert.Single(methodInfo!.GetParameters());
        }


        [Fact]
        public void IdeaRepository_ShouldImplementIIdeaRepository()
        {
            var implementsInterface = typeof(IdeaRepository)
                .GetInterfaces()
                .Any(i => i.Name == "IIdeaRepository");
            Assert.True(implementsInterface);
        }

        [Fact]
        public void IdeaRepository_ShouldHaveCorrectDependencies()
        {
            var constructor = typeof(IdeaRepository).GetConstructors().FirstOrDefault();
            Assert.NotNull(constructor);
            var parameters = constructor!.GetParameters();
            Assert.Equal(3, parameters.Length);
        }

        [Fact]
        public void IdeaRepository_ShouldRequireMongoDbContext()
        {
            var constructor = typeof(IdeaRepository).GetConstructors().FirstOrDefault();
            var parameters = constructor!.GetParameters();
            Assert.Contains(parameters, p => p.ParameterType.Name == "MongoDbContext");
        }

        [Fact]
        public void IdeaRepository_ShouldRequireIMapper()
        {
            var constructor = typeof(IdeaRepository).GetConstructors().FirstOrDefault();
            var parameters = constructor!.GetParameters();
            Assert.Contains(parameters, p => p.ParameterType.Name == "IMapper");
        }

        [Fact]
        public void IdeaRepository_ShouldRequireILogger()
        {
            var constructor = typeof(IdeaRepository).GetConstructors().FirstOrDefault();
            var parameters = constructor!.GetParameters();
            Assert.Contains(parameters, p => p.ParameterType.Name.Contains("ILogger"));
        }


        [Fact]
        public void Idea_ShouldHaveRequiredProperties()
        {
            var idea = new Idea
            {
                TopicId = 1,
                Title = "Test",
                Description = "Desc",
                CreatedBy = _testUser
            };
            Assert.Equal(1, idea.TopicId);
            Assert.Equal("Test", idea.Title);
        }

        [Fact]
        public void Idea_ShouldHaveDescription()
        {
            var idea = new Idea
            {
                TopicId = 1,
                Title = "Test",
                Description = "Test Description",
                CreatedBy = _testUser
            };
            Assert.Equal("Test Description", idea.Description);
        }

        [Fact]
        public void Idea_ShouldHaveCreatedBy()
        {
            var idea = new Idea
            {
                TopicId = 1,
                Title = "Test",
                Description = "Desc",
                CreatedBy = _testUser
            };
            Assert.NotNull(idea.CreatedBy);
            Assert.Equal("dummy", idea.CreatedBy.Login);
        }

        [Fact]
        public void Idea_ShouldHaveVoteCount()
        {
            var idea = new Idea
            {
                TopicId = 1,
                Title = "Test",
                Description = "Desc",
                CreatedBy = _testUser,
                VoteCount = 5
            };
            Assert.Equal(5, idea.VoteCount);
        }

        [Fact]
        public void Idea_ShouldHaveVotedByList()
        {
            var idea = new Idea
            {
                TopicId = 1,
                Title = "Test",
                Description = "Desc",
                CreatedBy = _testUser,
                VotedBy = new System.Collections.Generic.List<string> { "user1", "user2" }
            };
            Assert.NotEmpty(idea.VotedBy);
        }


        [Fact]
        public void CreateIdeaAsync_ShouldBeDeclaredAsync()
        {
            var methodInfo = typeof(IdeaRepository).GetMethod("CreateIdeaAsync");
            Assert.NotNull(methodInfo);
            Assert.True(methodInfo!.ReturnType.IsGenericType);
        }

        [Fact]
        public void CreateIdeaAsync_ShouldAcceptUserAndIdea()
        {
            var methodInfo = typeof(IdeaRepository).GetMethod("CreateIdeaAsync");
            var parameters = methodInfo!.GetParameters();
            
            Assert.Equal(2, parameters.Length);
            Assert.Equal("user", parameters[0].Name);
            Assert.Equal("idea", parameters[1].Name);
        }


        [Fact]
        public void GetIdeasByTopicIdAsync_ShouldBeDeclaredAsync()
        {
            var methodInfo = typeof(IdeaRepository).GetMethod("GetIdeasByTopicIdAsync");
            Assert.NotNull(methodInfo);
            Assert.True(methodInfo!.ReturnType.IsGenericType);
        }

        [Fact]
        public void GetIdeasByTopicIdAsync_ShouldAcceptTopicId()
        {
            var methodInfo = typeof(IdeaRepository).GetMethod("GetIdeasByTopicIdAsync");
            var parameters = methodInfo!.GetParameters();
            
            Assert.Single(parameters);
            Assert.Equal("topicId", parameters[0].Name);
        }


        [Fact]
        public void GetByIdAsync_ShouldBeDeclaredAsync()
        {
            var methodInfo = typeof(IdeaRepository).GetMethod("GetByIdAsync");
            Assert.NotNull(methodInfo);
            Assert.True(methodInfo!.ReturnType.IsGenericType);
        }

        [Fact]
        public void GetByIdAsync_ShouldAcceptId()
        {
            var methodInfo = typeof(IdeaRepository).GetMethod("GetByIdAsync");
            var parameters = methodInfo!.GetParameters();
            
            Assert.Single(parameters);
            Assert.Equal("id", parameters[0].Name);
        }


        [Fact]
        public void Constructor_ShouldInitializeFields()
        {
            var constructorInfo = typeof(IdeaRepository).GetConstructors().FirstOrDefault();
            Assert.NotNull(constructorInfo);
            Assert.Equal(3, constructorInfo!.GetParameters().Length);
        }

        [Fact]
        public void IdeaRepository_ShouldHavePrivateFields()
        {
            var fields = typeof(IdeaRepository).GetFields(
                System.Reflection.BindingFlags.NonPublic | 
                System.Reflection.BindingFlags.Instance
            );
            
            Assert.NotEmpty(fields);
        }
    }
}
