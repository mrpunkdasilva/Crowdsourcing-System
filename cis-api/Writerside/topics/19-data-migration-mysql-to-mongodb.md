# Data Migration from MySQL to MongoDB

This documentation describes different approaches for migrating data from a MySQL relational database to a MongoDB NoSQL database. The choice of the right approach depends on several factors, such as the complexity of the data, the volume of data, the time available, and the team's familiarity with the technologies.

## Migration Options

There are three main approaches for migrating data from MySQL to MongoDB:

1.  **Third-Party Tools (ETL and Dedicated Tools)**
2.  **Custom Scripts**
3.  **Queries (Export/Import)**

---

## 1. Third-Party Tools

Third-party tools, such as ETL (Extract, Transform, Load) tools and database IDEs, can simplify and accelerate the migration process. They usually provide a graphical interface to map data from MySQL to MongoDB and automate the migration process.

### Popular Free and Open Source Tools

*   **Talend Open Studio for Data Integration:** An open-source data integration tool with a user-friendly graphical interface for creating data pipelines, including MySQL to MongoDB migration.
*   **Apache NiFi:** A powerful open-source tool for data integration and automation, which supports MySQL to MongoDB migration through a drag-and-drop interface.
*   **Pentaho Data Integration:** Similar to Talend and NiFi, this is an open-source data integration tool that also supports this migration.
*   **MongoDB Compass:** The official GUI for MongoDB. It has a data import feature that can import data from JSON or CSV files, which can be generated from MySQL. The community edition is free to use.
*   **Mongify:** An open-source Ruby gem that reads your MySQL database, generates a translation file, and allows you to map how the data should be transformed for MongoDB.

### Advantages (Third-Party Tools)

*   **Ease of Use:** They often have intuitive graphical interfaces that do not require much programming knowledge.
*   **Speed:** Can be very fast for simple migrations.
*   **Additional Features:** Many tools offer features like scheduling, monitoring, and data transformation.

### Disadvantages (Third-Party Tools)

*   **Limited Flexibility:** May not be flexible enough for very complex data transformations.
*   **Learning Curve:** More complex tools can have a steep learning curve.

---

## 2. Custom Scripts

Writing a custom script for data migration offers maximum flexibility. You can write the script in your preferred programming language, such as Java (for the `users-api`) or C# (for the `cis-api`).

### Approach

The general process for a custom script is:

1.  **Connect to MySQL:** Use a database connector to connect to MySQL.
2.  **Read Data:** Execute SQL queries to read data from the tables.
3.  **Transform Data:** Map the relational data to a NoSQL document structure. This is the most complex step, where you can denormalize data, create nested documents, etc.
4.  **Connect to MongoDB:** Use a MongoDB driver to connect to MongoDB.
5.  **Write Data:** Insert the transformed documents into MongoDB.

### C# Example (for the `cis-api`)

A .NET console application can be used to read from MySQL (using Dapper or Entity Framework Core) and write to MongoDB (using the official MongoDB C# Driver).

```csharp
// Conceptual example of a migration script in C#

using System.Collections.Generic;
using Dapper;
using MySql.Data.MySqlClient;
using MongoDB.Driver;

public class MigrationService
{
    private readonly string _mysqlConnectionString = "...";
    private readonly string _mongoConnectionString = "...";

    public void MigrateUsers()
    {
        // 1. Connect and read from MySQL
        using var mysqlConnection = new MySqlConnection(_mysqlConnectionString);
        var usersFromMysql = mysqlConnection.Query("SELECT id, username, email FROM users");

        // 2. Connect to MongoDB
        var mongoClient = new MongoClient(_mongoConnectionString);
        var database = mongoClient.GetDatabase("my-mongo-db");
        var collection = database.GetCollection<UserMongoDb>("users");

        var usersToInsert = new List<UserMongoDb>();
        foreach (var user in usersFromMysql)
        {
            // 3. Transform data
            var userMongo = new UserMongoDb
            {
                LegacyId = user.id,
                Username = user.username,
                Email = user.email
            };
            usersToInsert.Add(userMongo);
        }

        // 4. Write to MongoDB
        if (usersToInsert.Any())
        {
            collection.InsertMany(usersToInsert);
        }
    }
}

public class UserMongoDb
{
    public int LegacyId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
}
```

### Java Example (for the `users-api`)

A Spring Boot application can be configured to connect to both databases. You can use Spring Data JPA to read from MySQL and Spring Data MongoDB to write to MongoDB.

```java
// Conceptual example of a migration service in Java with Spring Boot

@Service
public class MigrationService {

    @Autowired
    private UserRepositoryMysql userRepositoryMysql;

    @Autowired
    private UserRepositoryMongo userRepositoryMongo;

    public void migrateUsers() {
        // 1. Read from MySQL
        List<UserMysql> usersFromMysql = userRepositoryMysql.findAll();

        // 2. Transform and write to MongoDB
        List<UserMongo> usersToInsert = usersFromMysql.stream()
                .map(this::transformUser)
                .collect(Collectors.toList());

        if (!usersToInsert.isEmpty()) {
            userRepositoryMongo.saveAll(usersToInsert);
        }
    }

    private UserMongo transformUser(UserMysql userMysql) {
        UserMongo userMongo = new UserMongo();
        userMongo.setLegacyId(userMysql.getId());
        userMongo.setUsername(userMysql.getUsername());
        userMongo.setEmail(userMysql.getEmail());
        return userMongo;
    }
}
```

### Advantages (Custom Scripts)

*   **Maximum Flexibility:** Full control over the data transformation process.
*   **Cost:** No software licensing costs.
*   **Reusable:** The script can be modified and reused for future migrations.

### Disadvantages (Custom Scripts)

*   **Time and Effort:** Requires more development and testing time.
*   **Complexity:** Requires programming knowledge and familiarity with the database APIs.

---

## 3. Using Queries (Export/Import)

The simplest approach is to export data from MySQL to an intermediate format (like CSV or JSON) and then import that data into MongoDB.

### Tools

*   `mysqldump`: A command-line utility for exporting data from a MySQL database.
*   `mongoimport`: A command-line utility for importing data into a MongoDB collection.

### Process

1.  **Export from MySQL to CSV:**
    ```bash
    mysql -u <user> -p<password> -h <host> <database> -e "SELECT * FROM users;" | sed 's/\t/","/g;s/^/"/;s/$/"/' > users.csv
    ```

2.  **Import into MongoDB from CSV:**
    ```bash
    mongoimport --db my-mongo-db --collection users --type csv --headerline --file users.csv
    ```

### Advantages (Queries)

*   **Simplicity:** Very straightforward for direct data migrations (1 table to 1 collection).
*   **Speed:** Can be very fast for small to medium-sized datasets.

### Disadvantages (Queries)

*   **Limited Transformation:** Data transformation is very limited. Not ideal if you need to denormalize or restructure the data.
*   **Manual:** The process is manual and prone to errors.
*   **Doesn't Handle Relationships:** Does not handle migrating relationships between tables well.

---

## Comparison and Recommendation

| Criteria | Third-Party Tools | Custom Scripts | Queries (Export/Import) |
| :--- | :--- | :--- | :--- |
| **Ease of Use** | High (with GUI) | Medium/Low | Medium |
| **Flexibility** | Medium | High | Low |
| **Cost** | Free (for open source) | Low (development cost) | Free |
| **Performance** | Varies | Varies | Good for simple data |
| **Maintenance** | Low | Medium | High (for repeated processes) |

### Recommendation

Given the requirement for a **data migration pipeline**, the **Custom Scripts** approach is now the most recommended method for this project.

While Third-Party Tools offer simplicity for initial investigations, building a robust and automated pipeline often demands greater control over data transformation, error handling, and integration with existing CI/CD processes. Custom scripts, developed in C# or Java, provide the maximum flexibility to:

*   Handle complex data transformations from the relational MySQL model to the document-oriented MongoDB model.
*   Integrate seamlessly with the existing `cis-api` and `users-api` development ecosystem.
*   Be versioned and managed alongside the application code.
*   Be automated as part of a CI/CD pipeline for repeatable and reliable migrations.

Alternatively, robust **open-source ETL tools** like **Talend Open Studio** or **Apache NiFi** can also be considered for building data pipelines, especially if a visual interface for pipeline orchestration is preferred. However, custom scripts offer unparalleled control and adaptability for specific project needs.
