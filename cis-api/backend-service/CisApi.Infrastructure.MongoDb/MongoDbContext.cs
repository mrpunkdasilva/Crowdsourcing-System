using MongoDB.Driver;
using CisApi.Infrastructure.MongoDb.Entities;

namespace CisApi.Infrastructure.MongoDb
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IMongoClient client)
        {
            _database = client.GetDatabase("test");
            Console.WriteLine("[MONGO] Database: test");
        }

        public IMongoCollection<UserEntity> Users => _database.GetCollection<UserEntity>("users");
        public IMongoCollection<TopicEntity> Topics => _database.GetCollection<TopicEntity>("topics");
        public IMongoCollection<IdeaEntity> Ideas => _database.GetCollection<IdeaEntity>("ideas");
        public IMongoCollection<IdeaVotesEntity> IdeaVotes => _database.GetCollection<IdeaVotesEntity>("idea_votes");
    }
}
