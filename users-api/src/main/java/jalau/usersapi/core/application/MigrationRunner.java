package jalau.usersapi.core.application;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.boot.CommandLineRunner;
import org.springframework.context.annotation.Profile;
import org.springframework.stereotype.Component;

@Component
@Profile("migration")
public class MigrationRunner implements CommandLineRunner {

    private static final Logger logger = LoggerFactory.getLogger(MigrationRunner.class);
    private final UserMigrationService userMigrationService;

    public MigrationRunner(UserMigrationService userMigrationService) {
        this.userMigrationService = userMigrationService;
    }

    @Override
    public void run(String... args) throws Exception {
        logger.info("Starting user data migration from MySQL to MongoDB...");
        userMigrationService.migrateUsers();
        logger.info("User data migration completed.");
    }
}
