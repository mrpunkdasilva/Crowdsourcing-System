using Xunit;
using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using CisApi.Core.Domain.Entities;
using CisApi.Infrastructure.MySQL;
using CisApi.Infrastructure.MySQL.Repositories;
using CisApi.Infrastructure.MySQL.Entities;
using CisApi.Infrastructure.MySQL.MappingProfiles;

namespace CisApi.Infrastructure.MySQL.Tests;

public class TopicRepositoryTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var context = new AppDbContext(options);
        context.Database.EnsureCreated();
        return context;
    }

    private IMapper GetMapper()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<TopicProfile>();
            cfg.AddProfile<UserProfile>();
            cfg.AddProfile<IdeaProfile>();
        });
        return config.CreateMapper();
    }

    [Fact]
    public async Task CreateTopicAsync_ShouldCreateTopicSuccessfully()
    {
        // Arrange
        using var context = GetDbContext();
        var mapper = GetMapper();
        var repository = new TopicRepository(context, mapper);

        var userEntity = new UserEntity { Id = "user1", Login = "testuser" };
        context.Users.Add(userEntity);
        await context.SaveChangesAsync();

        var user = new User { Id = "user1", Login = "testuser" };
        var topic = new Topic { Title = "Test Topic", Description = "Description for test topic", CreatedBy = user };

        // Act
        var result = await repository.CreateTopicAsync(user, topic);

        // Assert
        Assert.NotNull(result);
        Assert.NotEqual(0, result.Id);
        Assert.Equal(topic.Title, result.Title);
        Assert.Equal(topic.Description, result.Description);
        Assert.Equal(user.Id, result.CreatedBy.Id);
        Assert.Equal(user.Login, result.CreatedBy.Login);

        var savedTopic = await context.Topics.Include(t => t.CreatedBy).FirstOrDefaultAsync(t => t.Id == result.Id);
        Assert.NotNull(savedTopic);
        Assert.Equal(topic.Title, savedTopic.Title);
        Assert.Equal(userEntity.Id, savedTopic.CreatedById);
    }

    [Fact]
    public async Task CreateTopicAsync_ShouldThrowInvalidOperationException_WhenUserNotFound()
    {
        // Arrange
        using var context = GetDbContext();
        var mapper = GetMapper();
        var repository = new TopicRepository(context, mapper);

        var user = new User { Id = "nonexistent", Login = "nonexistent" };
        var topic = new Topic { Title = "Test Topic", Description = "Description for test topic", CreatedBy = user };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => repository.CreateTopicAsync(user, topic));
    }

    [Fact]
    public async Task CreateTopicAsync_ShouldThrowArgumentNullException_WhenUserIsNull()
    {
        // Arrange
        using var context = GetDbContext();
        var mapper = GetMapper();
        var repository = new TopicRepository(context, mapper);

        User nullUser = null!;
        var topic = new Topic { Title = "Test Topic", Description = "Description for test topic", CreatedBy = new User { Id = "dummy", Login = "dummy" } };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => repository.CreateTopicAsync(nullUser, topic));
    }

    [Fact]
    public async Task CreateTopicAsync_ShouldThrowArgumentNullException_WhenTopicIsNull()
    {
        // Arrange
        using var context = GetDbContext();
        var mapper = GetMapper();
        var repository = new TopicRepository(context, mapper);

        var userEntity = new UserEntity { Id = "user1", Login = "testuser" };
        context.Users.Add(userEntity);
        await context.SaveChangesAsync();

        var user = new User { Id = "user1", Login = "testuser" };
        Topic nullTopic = null!;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(
            () => repository.CreateTopicAsync(user, nullTopic));
    }
}