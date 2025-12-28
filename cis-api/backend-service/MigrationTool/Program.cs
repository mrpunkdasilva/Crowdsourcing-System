using System;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using MongoDB.Driver;
using MongoDB.Bson;

namespace MigrationTool
{
    class Program
    {
        private const string MYSQL_CONN = "Server=localhost;Port=3307;Database=sd3;User=root;Password=sd5;";
        private const string MONGO_CONN = "mongodb://localhost:27017";
        private const string MONGO_DB = "test";

        static async Task Main(string[] args)
        {
            Console.WriteLine("🔄 CIS API - Migration Tool (MySQL → MongoDB)");
            Console.WriteLine("==============================================\n");

            try
            {
                await MigrateData();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ ERRO: {ex.Message}");
            }

            Console.WriteLine("\n✅ Pressione qualquer tecla para sair...");
            Console.ReadKey();
        }

        static async Task MigrateData()
        {
            using var mysqlConn = new MySqlConnection(MYSQL_CONN);
            await mysqlConn.OpenAsync();
            Console.WriteLine("✅ Conectado ao MySQL");

            var mongoClient = new MongoClient(MONGO_CONN);
            var mongoDb = mongoClient.GetDatabase(MONGO_DB);
            Console.WriteLine($"✅ Conectado ao MongoDB (Database: {MONGO_DB})\n");

            await MigrateUsers(mysqlConn, mongoDb);    // ← ADICIONADO!
            await MigrateTopics(mysqlConn, mongoDb);
            await MigrateIdeas(mysqlConn, mongoDb);

            Console.WriteLine("\n📊 RELATÓRIO FINAL:");
            var usersCount = await mongoDb.GetCollection<BsonDocument>("users").CountDocumentsAsync(new BsonDocument());
            var topicsCount = await mongoDb.GetCollection<BsonDocument>("topics").CountDocumentsAsync(new BsonDocument());
            var ideasCount = await mongoDb.GetCollection<BsonDocument>("ideas").CountDocumentsAsync(new BsonDocument());
            
            Console.WriteLine($"Users migrados: {usersCount}");
            Console.WriteLine($"Topics migrados: {topicsCount}");
            Console.WriteLine($"Ideas migradas: {ideasCount}");
        }

        static async Task MigrateUsers(MySqlConnection mysqlConn, IMongoDatabase mongoDb)
        {
            Console.WriteLine("👤 Migrando USERS...");
            
            var collection = mongoDb.GetCollection<BsonDocument>("users");
            await collection.DeleteManyAsync(new BsonDocument());
            
            var cmd = new MySqlCommand("SELECT * FROM users", mysqlConn);
            using var reader = await cmd.ExecuteReaderAsync();
            
            var documents = new System.Collections.Generic.List<BsonDocument>();
            
            while (await reader.ReadAsync())
            {
                var doc = new BsonDocument
                {
                    { "id", reader.GetString(reader.GetOrdinal("id")) },
                    { "login", reader.GetString(reader.GetOrdinal("login")) }
                };
                
                documents.Add(doc);
            }
            
            if (documents.Count > 0)
            {
                await collection.InsertManyAsync(documents);
                Console.WriteLine($"✅ {documents.Count} users migrados");
            }
            else
            {
                Console.WriteLine("⚠️ Nenhum user encontrado");
            }
        }

        static async Task MigrateTopics(MySqlConnection mysqlConn, IMongoDatabase mongoDb)
        {
            Console.WriteLine("\n📋 Migrando TOPICS...");
            
            var collection = mongoDb.GetCollection<BsonDocument>("topics");
            await collection.DeleteManyAsync(new BsonDocument());
            
            var cmd = new MySqlCommand("SELECT * FROM topics", mysqlConn);
            using var reader = await cmd.ExecuteReaderAsync();
            
            var documents = new System.Collections.Generic.List<BsonDocument>();
            
            while (await reader.ReadAsync())
            {
                var doc = new BsonDocument
                {
                    { "id", reader.GetInt32(reader.GetOrdinal("id")) },
                    { "title", reader.GetString(reader.GetOrdinal("title")) },
                    { "description", reader.IsDBNull(reader.GetOrdinal("description")) ? "" : reader.GetString(reader.GetOrdinal("description")) },
                    { "created_by_id", reader.IsDBNull(reader.GetOrdinal("created_by_id")) ? "" : reader.GetString(reader.GetOrdinal("created_by_id")) },
                    { "created_at", reader.GetDateTime(reader.GetOrdinal("created_at")) }
                };
                
                documents.Add(doc);
            }
            
            if (documents.Count > 0)
            {
                await collection.InsertManyAsync(documents);
                Console.WriteLine($"✅ {documents.Count} topics migrados");
            }
            else
            {
                Console.WriteLine("⚠️ Nenhum topic encontrado");
            }
        }

        static async Task MigrateIdeas(MySqlConnection mysqlConn, IMongoDatabase mongoDb)
        {
            Console.WriteLine("\n💡 Migrando IDEAS...");
            
            var collection = mongoDb.GetCollection<BsonDocument>("ideas");
            await collection.DeleteManyAsync(new BsonDocument());
            
            var cmd = new MySqlCommand("SELECT * FROM ideas", mysqlConn);
            using var reader = await cmd.ExecuteReaderAsync();
            
            var documents = new System.Collections.Generic.List<BsonDocument>();
            
            while (await reader.ReadAsync())
            {
                var doc = new BsonDocument
                {
                    { "id", reader.GetInt32(reader.GetOrdinal("id")) },
                    { "topic_id", reader.GetInt32(reader.GetOrdinal("topic_id")) },
                    { "title", reader.GetString(reader.GetOrdinal("title")) },
                    { "description", reader.IsDBNull(reader.GetOrdinal("description")) ? "" : reader.GetString(reader.GetOrdinal("description")) },
                    { "created_by_id", reader.IsDBNull(reader.GetOrdinal("created_by_id")) ? "" : reader.GetString(reader.GetOrdinal("created_by_id")) },
                    { "vote_count", reader.IsDBNull(reader.GetOrdinal("vote_count")) ? 0 : reader.GetInt32(reader.GetOrdinal("vote_count")) },
                    { "created_at", reader.GetDateTime(reader.GetOrdinal("created_at")) },
                    { "created_by", new BsonDocument 
                        {
                            { "mongo_id", "" },
                            { "login", "migrated" }
                        }
                    },
                    { "topic", new BsonDocument 
                        {
                            { "title", "" },
                            { "description", "" },
                            { "created_by_id", "" }
                        }
                    }
                };
                
                documents.Add(doc);
            }
            
            if (documents.Count > 0)
            {
                await collection.InsertManyAsync(documents);
                Console.WriteLine($"✅ {documents.Count} ideas migradas");
            }
            else
            {
                Console.WriteLine("⚠️ Nenhuma idea encontrada");
            }
        }
    }
}
