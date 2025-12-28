package jalau.usersapi.core.application;

import jalau.usersapi.infrastructure.mongodb.entities.DbUser;
import jalau.usersapi.infrastructure.mongodb.repositories.SpringDataMongoUserRepository;
import jalau.usersapi.infrastructure.mysql.SpringDataUserRepository;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.context.annotation.Profile;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import org.springframework.data.mongodb.core.MongoTemplate;

import java.util.List;
import java.util.stream.Collectors;

@Service
@Profile("migration")
public class UserMigrationService {

    private static final Logger logger = LoggerFactory.getLogger(UserMigrationService.class);
    private final SpringDataUserRepository mysqlUserRepository;
    private final SpringDataMongoUserRepository mongoUserRepository;
    private final MongoTemplate mongoTemplate;

    public UserMigrationService(SpringDataUserRepository mysqlUserRepository,
                                SpringDataMongoUserRepository mongoUserRepository,
                                MongoTemplate mongoTemplate) {
        this.mysqlUserRepository = mysqlUserRepository;
        this.mongoUserRepository = mongoUserRepository;
        this.mongoTemplate = mongoTemplate;
    }

    @Transactional
    public void migrateUsers() {
        if (mongoTemplate.collectionExists("users")) {
            mongoTemplate.dropCollection("users");
            logger.info("Dropped existing 'users' collection in MongoDB.");
        }
        logger.info("MongoDB connected to database: {}", mongoTemplate.getDb().getName());

        List<jalau.usersapi.infrastructure.mysql.entities.DbUser> mysqlUsers = mysqlUserRepository.findAll();

        List<DbUser> mongoUsers = mysqlUsers.stream()
                .map(this::mapToMongoDbUser)
                .collect(Collectors.toList());

        if (!mongoUsers.isEmpty()) {
            mongoUserRepository.saveAll(mongoUsers);
            logger.info("Migrated {} users from MySQL to MongoDB.", mongoUsers.size());
        } else {
            logger.info("No users to migrate from MySQL.");
        }

        verifyMigration();
    }

    private void verifyMigration() {
        logger.info("Starting data integrity verification...");
        long mysqlUserCount = mysqlUserRepository.count();
        long mongoUserCount = mongoUserRepository.count();

        logger.info("Source user count (MySQL): {}", mysqlUserCount);
        logger.info("Destination user count (MongoDB): {}", mongoUserCount);

        if (mysqlUserCount == mongoUserCount) {
            logger.info("VERIFICATION SUCCESS: Row counts match.");
        } else {
            logger.warn("VERIFICATION FAILED: Row counts do not match!");
        }
    }

    private DbUser mapToMongoDbUser(jalau.usersapi.infrastructure.mysql.entities.DbUser mysqlUser) {
        DbUser mongoUser = new DbUser();
        mongoUser.setSql_id(mysqlUser.getId());
        mongoUser.setName(mysqlUser.getName());
        mongoUser.setLogin(mysqlUser.getLogin());
        mongoUser.setPassword(mysqlUser.getPassword());
        return mongoUser;
    }
}
