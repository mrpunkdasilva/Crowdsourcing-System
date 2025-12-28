using Xunit;
using CisApi.Infrastructure.MongoDb.Repositories;
using System.Linq;

namespace CisApi.Infrastructure.MongoDB.Tests.Repositories
{
    public class VoteRepositoryTests
    {

        [Fact]
        public void CreateVoteAsync_ShouldHaveCorrectSignature()
        {
            var methodInfo = typeof(VoteRepository).GetMethod("CreateVoteAsync");
            Assert.NotNull(methodInfo);
            Assert.Equal(2, methodInfo!.GetParameters().Length);
        }

        [Fact]
        public void DeleteVoteAsync_ShouldHaveCorrectSignature()
        {
            var methodInfo = typeof(VoteRepository).GetMethod("DeleteVoteAsync");
            Assert.NotNull(methodInfo);
            Assert.Equal(2, methodInfo!.GetParameters().Length);
        }


        [Fact]
        public void VoteRepository_ShouldImplementIVoteRepository()
        {
            var implementsInterface = typeof(VoteRepository)
                .GetInterfaces()
                .Any(i => i.Name == "IVoteRepository");
            Assert.True(implementsInterface);
        }

        [Fact]
        public void VoteRepository_ShouldHaveThreeDependencies()
        {
            var constructor = typeof(VoteRepository).GetConstructors().FirstOrDefault();
            Assert.NotNull(constructor);
            Assert.Equal(3, constructor!.GetParameters().Length);
        }

        [Fact]
        public void VoteRepository_ShouldRequireMongoDbContext()
        {
            var constructor = typeof(VoteRepository).GetConstructors().FirstOrDefault();
            var parameters = constructor!.GetParameters();
            Assert.Contains(parameters, p => p.ParameterType.Name == "MongoDbContext");
        }

        [Fact]
        public void VoteRepository_ShouldRequireIMapper()
        {
            var constructor = typeof(VoteRepository).GetConstructors().FirstOrDefault();
            var parameters = constructor!.GetParameters();
            Assert.Contains(parameters, p => p.ParameterType.Name == "IMapper");
        }

        [Fact]
        public void VoteRepository_ShouldRequireILogger()
        {
            var constructor = typeof(VoteRepository).GetConstructors().FirstOrDefault();
            var parameters = constructor!.GetParameters();
            Assert.Contains(parameters, p => p.ParameterType.Name.Contains("ILogger"));
        }


        [Fact]
        public void CreateVoteAsync_ShouldBeDeclaredAsync()
        {
            var methodInfo = typeof(VoteRepository).GetMethod("CreateVoteAsync");
            Assert.NotNull(methodInfo);
            Assert.True(methodInfo!.ReturnType.IsGenericType);
            Assert.True(methodInfo.ReturnType.GetGenericTypeDefinition().Name.Contains("Task"));
        }

        [Fact]
        public void CreateVoteAsync_ShouldAcceptUserAndIdeaId()
        {
            var methodInfo = typeof(VoteRepository).GetMethod("CreateVoteAsync");
            var parameters = methodInfo!.GetParameters();
            
            Assert.Equal("user", parameters[0].Name);
            Assert.Equal("ideaId", parameters[1].Name);
        }

        [Fact]
        public void CreateVoteAsync_ShouldReturnIdea()
        {
            var methodInfo = typeof(VoteRepository).GetMethod("CreateVoteAsync");
            Assert.NotNull(methodInfo);
            var returnType = methodInfo!.ReturnType.GenericTypeArguments.FirstOrDefault();
            Assert.NotNull(returnType);
            Assert.Equal("Idea", returnType.Name);
        }


        [Fact]
        public void DeleteVoteAsync_ShouldBeDeclaredAsync()
        {
            var methodInfo = typeof(VoteRepository).GetMethod("DeleteVoteAsync");
            Assert.NotNull(methodInfo);
            Assert.True(methodInfo!.ReturnType.IsGenericType);
            Assert.True(methodInfo.ReturnType.GetGenericTypeDefinition().Name.Contains("Task"));
        }

        [Fact]
        public void DeleteVoteAsync_ShouldAcceptUserAndIdeaId()
        {
            var methodInfo = typeof(VoteRepository).GetMethod("DeleteVoteAsync");
            var parameters = methodInfo!.GetParameters();
            
            Assert.Equal("user", parameters[0].Name);
            Assert.Equal("ideaId", parameters[1].Name);
        }

        [Fact]
        public void DeleteVoteAsync_ShouldReturnIdea()
        {
            var methodInfo = typeof(VoteRepository).GetMethod("DeleteVoteAsync");
            Assert.NotNull(methodInfo);
            var returnType = methodInfo!.ReturnType.GenericTypeArguments.FirstOrDefault();
            Assert.NotNull(returnType);
            Assert.Equal("Idea", returnType.Name);
        }


        
        [Fact]
        public void Constructor_ShouldInitializeFields()
        {
            var constructorInfo = typeof(VoteRepository).GetConstructors().FirstOrDefault();
            Assert.NotNull(constructorInfo);
            Assert.Equal(3, constructorInfo!.GetParameters().Length);
        }

        [Fact]
        public void VoteRepository_ShouldHavePrivateFields()
        {
            var fields = typeof(VoteRepository).GetFields(
                System.Reflection.BindingFlags.NonPublic | 
                System.Reflection.BindingFlags.Instance
            );
            
            Assert.NotEmpty(fields);
            Assert.True(fields.Any(f => f.Name.Contains("context") || f.Name.Contains("Context")));
        }
    }
}
