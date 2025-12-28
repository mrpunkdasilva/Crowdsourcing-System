# How Data Migration Works

This document details the process of migrating user data from the MySQL database to MongoDB. The migration is implemented as a script that is executed on-demand using a specific Spring Boot profile.

The goal is to ensure that user data is transferred consistently and safely from the relational environment to the NoSQL environment.

## How to Run the Migration

To execute the migration process, the Spring Boot application must be started with the `migration` profile activated. This ensures that the application runs as a single-task script instead of starting the web server.

Run the following command in the project root:

```bash
mvn spring-boot:run -Pmigration
```

When executed, the script will print messages to the console, indicating the start and end of the process, as well as the number of users migrated.

## What Happens Step-by-Step

The migration process is orchestrated by a few specific classes within the application. Below is a breakdown of each step.

### 1. The Trigger: `MigrationRunner.java`

The entry point for the migration process is the `MigrationRunner` class.

-   **Profile Activation**: This class is annotated with `@Profile("migration")`, which means it is only loaded and executed by Spring Boot when the `migration` profile is active.
-   **Automatic Execution**: By implementing the `CommandLineRunner` interface, its `run()` method is automatically called as soon as the application starts.
-   **Delegation**: The sole responsibility of `MigrationRunner` is to call the `migrateUsers()` method of the `UserMigrationService` class, which contains the main migration logic.

### 2. The Core Logic: `UserMigrationService.java`

This class contains the main workflow of the migration.

#### a. Cleaning the Old Collection

Before starting the migration, the service first checks if the `users` collection already exists in MongoDB. If it does, **it is completely deleted (`dropCollection`)**. This ensures that each migration run is "clean," avoiding duplicates or inconsistent data from previous runs.

#### b. Reading Data from MySQL

The service uses the `SpringDataUserRepository` (configured for MySQL) to fetch all user records from the table in the MySQL database.

#### c. Transforming the Data

Each user read from MySQL is transformed into a MongoDB `DbUser` object. The `mapToMongoDbUser` method is responsible for this conversion:

```java
private DbUser mapToMongoDbUser(jalau.usersapi.infrastructure.mysql.entities.DbUser mysqlUser) {
    DbUser mongoUser = new DbUser();
    mongoUser.setSql_id(mysqlUser.getId());
    mongoUser.setName(mysqlUser.getName());
    mongoUser.setLogin(mysqlUser.getLogin());
    mongoUser.setPassword(mysqlUser.getPassword());
    return mongoUser;
}
```
- The original MySQL `id` is preserved in the `sql_id` field in MongoDB.
- The `name`, `login`, and `password` fields are copied directly.

#### d. Writing Data to MongoDB

After transforming all users, the list of new MongoDB `DbUser` objects is saved to the `users` collection all at once using the `saveAll()` method.

### 3. The Configuration: `application-migration.properties`

This file is crucial as it provides the connection settings for **both** databases:

-   **MySQL (`spring.datasource.*`)**: Points to the source database.
-   **MongoDB (`spring.data.mongodb.uri`)**: Points to the destination database.
-   **`spring.main.web-application-type=none`**: This property instructs Spring Boot not to start the web server (like Tomcat), treating the application as a simple command-line script.

## Relevant Files

-   `src/main/java/jalau/usersapi/core/application/MigrationRunner.java`: The trigger that starts the process.
-   `src/main/java/jalau/usersapi/core/application/UserMigrationService.java`: Contains the main logic for reading, transforming, and writing data.
-   `src/main/resources/application-migration.properties`: Configuration file with database connection data for the `migration` profile.