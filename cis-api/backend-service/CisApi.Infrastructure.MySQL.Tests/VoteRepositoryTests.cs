using Xunit;
using Moq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using CisApi.Infrastructure.MySQL.Repositories;
using CisApi.Infrastructure.MySQL;
using CisApi.Infrastructure.MySQL.Entities;
using CisApi.Core.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace CisApi.Infrastructure.MySQL.Tests.Repositories
{
    public class VoteRepositoryTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly Mock<IMapper> _mockMapper;
        private readonly VoteRepository _repository;
        private readonly User _testUser;
        private readonly UserEntity _testUserEntity;
        private readonly TopicEntity _testTopicEntity;
        private readonly IdeaEntity _testIdeaEntity;

        public VoteRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _mockMapper = new Mock<IMapper>();
            _repository = new VoteRepository(_context, _mockMapper.Object);
            
            _testUser = new User { Id = "1", Login = "testuser" };
            
            _testUserEntity = new UserEntity
            {
                Id = "1",
                Login = "testuser"
            };

            _testTopicEntity = new TopicEntity
            {
                Id = 1,
                Title = "Test Topic",
                Description = "Test",
                CreatedById = "1",
                CreatedAt = DateTime.UtcNow,
                CreatedBy = _testUserEntity
            };

            _testIdeaEntity = new IdeaEntity
            {
                Id = 1,
                Title = "Test Idea",
                Description = "Test",
                TopicId = 1,
                CreatedById = "1",
                VoteCount = 0,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = _testUserEntity,
                Topic = _testTopicEntity
            };
        }

        [Fact]
        public async Task CreateVoteAsync_ShouldCreateVote_WhenValid()
        {
            _context.Users.Add(_testUserEntity);
            _context.Topics.Add(_testTopicEntity);
            _context.Ideas.Add(_testIdeaEntity);
            await _context.SaveChangesAsync();

            var votedIdea = new Idea 
            { 
                Id = 1, 
                Title = "Test", 
                Description = "Test Desc",
                VoteCount = 1, 
                CreatedBy = _testUser 
            };
            _mockMapper.Setup(m => m.Map<Idea>(It.IsAny<IdeaEntity>())).Returns(votedIdea);

            var result = await _repository.CreateVoteAsync(_testUser, 1);

            Assert.NotNull(result);
            Assert.Equal(1, result.VoteCount);
            _mockMapper.Verify(m => m.Map<Idea>(It.IsAny<IdeaEntity>()), Times.Once);
        }

        [Fact]
        public async Task CreateVoteAsync_ShouldThrowException_WhenIdeaNotFound()
        {
            await Assert.ThrowsAsync<Exception>(
                async () => await _repository.CreateVoteAsync(_testUser, 999)
            );
        }

        [Fact]
        public async Task CreateVoteAsync_ShouldThrowException_WhenUserNotFound()
        {
            _context.Topics.Add(_testTopicEntity);
            _context.Ideas.Add(_testIdeaEntity);
            await _context.SaveChangesAsync();

            var invalidUser = new User { Id = "999", Login = "invalid" };

            await Assert.ThrowsAsync<Exception>(
                async () => await _repository.CreateVoteAsync(invalidUser, 1)
            );
        }

        [Fact]
        public async Task CreateVoteAsync_ShouldThrowException_WhenUserAlreadyVoted()
        {
            _context.Users.Add(_testUserEntity);
            _context.Topics.Add(_testTopicEntity);
            _context.Ideas.Add(_testIdeaEntity);
            await _context.SaveChangesAsync();

            var vote = new IdeaVotesEntity { IdeaId = 1, UserId = "1", Idea = _testIdeaEntity, User = _testUserEntity };
            _context.IdeaVotes.Add(vote);
            await _context.SaveChangesAsync();

            await Assert.ThrowsAsync<Exception>(
                async () => await _repository.CreateVoteAsync(_testUser, 1)
            );
        }

        [Fact]
        public async Task DeleteVoteAsync_ShouldRemoveVote_WhenExists()
        {
            _context.Users.Add(_testUserEntity);
            _context.Topics.Add(_testTopicEntity);
            _testIdeaEntity.VoteCount = 1;
            _context.Ideas.Add(_testIdeaEntity);
            await _context.SaveChangesAsync();

            var vote = new IdeaVotesEntity { IdeaId = 1, UserId = "1", Idea = _testIdeaEntity, User = _testUserEntity };
            _context.IdeaVotes.Add(vote);
            await _context.SaveChangesAsync();

            // ✅ Adicionar Description
            var unvotedIdea = new Idea 
            { 
                Id = 1, 
                Title = "Test", 
                Description = "Test Desc",
                VoteCount = 0, 
                CreatedBy = _testUser 
            };
            _mockMapper.Setup(m => m.Map<Idea>(It.IsAny<IdeaEntity>())).Returns(unvotedIdea);

            var result = await _repository.DeleteVoteAsync(_testUser, 1);

            Assert.NotNull(result);
            Assert.Equal(0, result.VoteCount);
            var votesCount = _context.IdeaVotes.Count();
            Assert.Equal(0, votesCount);
        }

        [Fact]
        public async Task DeleteVoteAsync_ShouldThrowException_WhenVoteNotFound()
        {
            _context.Users.Add(_testUserEntity);
            _context.Topics.Add(_testTopicEntity);
            _context.Ideas.Add(_testIdeaEntity);
            await _context.SaveChangesAsync();

            await Assert.ThrowsAsync<Exception>(
                async () => await _repository.DeleteVoteAsync(_testUser, 1)
            );
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
