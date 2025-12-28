using CisApi.Infrastructure.MongoDB.Documents;
using CisApi.Infrastructure.MySQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CisApi.DataMigration
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<Program>>();
                try
                {
                    logger.LogInformation("Starting data migration application.");
                    var migrator = services.GetRequiredService<MigrationService>();
                    await migrator.MigrateDataAsync();
                    logger.LogInformation("Migration completed successfully!");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred during migration: {Message}", ex.Message);
                }
            }

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddLogging(configure =>
                    {
                        configure.AddConsole();
                        configure.SetMinimumLevel(LogLevel.Information);
                    });

                    services.AddDbContext<AppDbContext>(options =>
                        options.UseMySql(hostContext.Configuration.GetConnectionString("DefaultConnection"),
                            Microsoft.EntityFrameworkCore.ServerVersion.AutoDetect(hostContext.Configuration.GetConnectionString("DefaultConnection"))));

                    services.AddSingleton<IMongoClient>(s =>
                        new MongoClient(hostContext.Configuration.GetConnectionString("MongoDb")));
                    services.AddSingleton<IMongoDatabase>(s =>
                    {
                        var client = s.GetRequiredService<IMongoClient>();
                        var databaseName = hostContext.Configuration.GetSection("MongoDb:DatabaseName").Get<string>();
                        return client.GetDatabase(databaseName);
                    });

                    services.AddTransient<MigrationService>();
                });
    }

    public class MigrationService
    {
        private readonly AppDbContext _mysqlDbContext;
        private readonly IMongoDatabase _mongoDatabase;
        private readonly ILogger<MigrationService> _logger;

        public MigrationService(AppDbContext mysqlDbContext, IMongoDatabase mongoDatabase, ILogger<MigrationService> logger)
        {
            _mysqlDbContext = mysqlDbContext;
            _mongoDatabase = mongoDatabase;
            _logger = logger;
        }

        public async Task MigrateDataAsync()
        {
            _logger.LogInformation("Starting data migration from MySQL to MongoDB...");

            // --- Migrate Users ---
            _logger.LogInformation("Migrating Users...");
            await _mongoDatabase.DropCollectionAsync("users");
            var usersCollection = _mongoDatabase.GetCollection<UserDocument>("users");
            var mysqlUsers = await _mysqlDbContext.Users.ToListAsync();
            var userDocuments = mysqlUsers.Select(u => new UserDocument
            {
                Id = u.Id,
                Login = u.Login
            }).ToList();

            if (userDocuments.Any())
            {
                await usersCollection.InsertManyAsync(userDocuments);
                _logger.LogInformation("Inserted {Count} user documents into MongoDB.", userDocuments.Count);
            }
            else
            {
                _logger.LogInformation("No users found in MySQL to migrate.");
            }

            // Create a map for MySQL User ID to MongoDB User ID for referencing
            var userMap = userDocuments.ToDictionary(u => u.Id, u => u.Id); // Mapeia MySqlId para o Id do MongoDB (que é o mesmo)

            // --- Migrate Topics ---
            _logger.LogInformation("Migrating Topics...");
            await _mongoDatabase.DropCollectionAsync("topics");
            var topicsCollection = _mongoDatabase.GetCollection<TopicDocument>("topics");
            var mysqlTopics = await _mysqlDbContext.Topics.Include(t => t.CreatedBy).ToListAsync();
            var topicDocuments = mysqlTopics.Select(t => new TopicDocument
            {
                MySqlId = t.Id,
                Title = t.Title,
                Description = t.Description,
                CreatedAt = t.CreatedAt,
                CreatedBy = new UserDocument
                {
                    Id = userMap.GetValueOrDefault(t.CreatedBy.Id, string.Empty),
                    Login = t.CreatedBy.Login
                }
            }).ToList();

            if (topicDocuments.Any())
            {
                await topicsCollection.InsertManyAsync(topicDocuments);
                _logger.LogInformation("Inserted {Count} topic documents into MongoDB.", topicDocuments.Count);
            }
            else
            {
                _logger.LogInformation("No topics found in MySQL to migrate.");
            }

            // Create a map for MySQL Topic ID to MongoDB Topic ID
            var topicMap = topicDocuments.ToDictionary(t => t.MySqlId, t => t.Id);

            // --- Migrate Ideas ---
            _logger.LogInformation("Migrating Ideas...");
            await _mongoDatabase.DropCollectionAsync("ideas");
            var ideasCollection = _mongoDatabase.GetCollection<IdeaDocument>("ideas");
            var mysqlIdeas = await _mysqlDbContext.Ideas.Include(i => i.CreatedBy).ToListAsync();
            var ideaDocuments = mysqlIdeas.Select(i => new IdeaDocument
            {
                MySqlId = i.Id,
                TopicId = topicMap.GetValueOrDefault(i.TopicId, string.Empty), // Use mapped MongoDB Topic ID
                Title = i.Title,
                Description = i.Description,
                CreatedAt = i.CreatedAt,
                CreatedBy = new UserDocument
                {
                    Id = userMap.GetValueOrDefault(i.CreatedBy.Id, string.Empty),
                    Login = i.CreatedBy.Login
                },
                VoteCount = (int)i.VoteCount // Cast para int, pois VoteCount é long no MySQL
            }).ToList();

            if (ideaDocuments.Any())
            {
                await ideasCollection.InsertManyAsync(ideaDocuments);
                _logger.LogInformation("Inserted {Count} idea documents into MongoDB.", ideaDocuments.Count);
            }
            else
            {
                _logger.LogInformation("No ideas found in MySQL to migrate.");
            }

            // Create a map for MySQL Idea ID to MongoDB Idea ID
            var ideaMap = ideaDocuments.ToDictionary(i => i.MySqlId, i => i.Id);

            // --- Migrate IdeaVotes ---
            _logger.LogInformation("Migrating IdeaVotes...");
            await _mongoDatabase.DropCollectionAsync("idea_votes");
            var ideaVotesCollection = _mongoDatabase.GetCollection<VoteDocument>("idea_votes");
            var mysqlIdeaVotes = await _mysqlDbContext.IdeaVotes.Include(iv => iv.User).ToListAsync();
            var voteDocuments = mysqlIdeaVotes.Select(iv => new VoteDocument
            {
                IdeaId = ideaMap.GetValueOrDefault(iv.IdeaId, string.Empty), // Use mapped MongoDB Idea ID
                UserId = userMap.GetValueOrDefault(iv.UserId, string.Empty), // Use mapped MongoDB User ID
                UserLogin = iv.User.Login,
                VotedAt = iv.VotedAt
            }).ToList();

            if (voteDocuments.Any())
            {
                await ideaVotesCollection.InsertManyAsync(voteDocuments);
                _logger.LogInformation("Inserted {Count} vote documents into MongoDB.", voteDocuments.Count);
            }
            else
            {
                _logger.LogInformation("No idea votes found in MySQL to migrate.");
            }

            _logger.LogInformation("Data migration completed. Performing integrity checks...");

            // --- Integrity Checks ---
            // Users
            var mysqlUserCount = await _mysqlDbContext.Users.CountAsync();
            var mongoUserCount = await usersCollection.CountDocumentsAsync(_ => true);
            _logger.LogInformation("MySQL User Count: {MySqlCount}, MongoDB User Count: {MongoCount}", mysqlUserCount, mongoUserCount);
            if (mysqlUserCount == mongoUserCount) _logger.LogInformation("User count integrity check PASSED.");
            else _logger.LogWarning("User count integrity check FAILED: MySQL has {MySqlCount} users, MongoDB has {MongoCount}.", mysqlUserCount, mongoUserCount);

            // Topics
            var mysqlTopicCount = await _mysqlDbContext.Topics.CountAsync();
            var mongoTopicCount = await topicsCollection.CountDocumentsAsync(_ => true);
            _logger.LogInformation("MySQL Topic Count: {MySqlCount}, MongoDB Topic Count: {MongoCount}", mysqlTopicCount, mongoTopicCount);
            if (mysqlTopicCount == mongoTopicCount) _logger.LogInformation("Topic count integrity check PASSED.");
            else _logger.LogWarning("Topic count integrity check FAILED: MySQL has {MySqlCount} topics, MongoDB has {MongoCount}.", mysqlTopicCount, mongoTopicCount);

            // Ideas
            var mysqlIdeaCount = await _mysqlDbContext.Ideas.CountAsync();
            var mongoIdeaCount = await ideasCollection.CountDocumentsAsync(_ => true);
            _logger.LogInformation("MySQL Idea Count: {MySqlCount}, MongoDB Idea Count: {MongoCount}", mysqlIdeaCount, mongoIdeaCount);
            if (mysqlIdeaCount == mongoIdeaCount) _logger.LogInformation("Idea count integrity check PASSED.");
            else _logger.LogWarning("Idea count integrity check FAILED: MySQL has {MySqlCount} ideas, MongoDB has {MongoCount}.", mysqlIdeaCount, mongoIdeaCount);

            // IdeaVotes
            var mysqlVoteCount = await _mysqlDbContext.IdeaVotes.CountAsync();
            var mongoVoteCount = await ideaVotesCollection.CountDocumentsAsync(_ => true);
            _logger.LogInformation("MySQL Vote Count: {MySqlCount}, MongoDB Vote Count: {MongoCount}", mysqlVoteCount, mongoVoteCount);
            if (mysqlVoteCount == mongoVoteCount) _logger.LogInformation("Vote count integrity check PASSED.");
            else _logger.LogWarning("Vote count integrity check FAILED: MySQL has {MySqlCount} votes, MongoDB has {MongoCount}.", mysqlVoteCount, mongoVoteCount);
        }
    }
}