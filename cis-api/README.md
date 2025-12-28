# CIS Api

## Table of Contents

*   [About](#about)
*   [Getting Started](#getting-started)
    *   [Prerequisites](#prerequisites)
    *   [Clone the Repository](#clone-the-repository)
    *   [Build the Solution](#build-the-solution)
    *   [Run the Project](#run-the-project)
    *   [Dependencies](#dependencies)
    *   [Testing and Code Coverage](#testing-and-code-coverage)

## About

This is the **CisApi** project, a RESTful API developed using C# ASP.NET Core 9.0.

## Getting Started

To start working with the CisApi project, follow the steps below:

### Prerequisites

Ensure you have the .NET 9.0 SDK installed. You can download it from [https://dotnet.microsoft.com/en-us/download](https://dotnet.microsoft.com/en-us/download).

### Clone the Repository

To get a local copy of the project, clone the repository using the following command:

```bash
git clone https://gitlab.com/jala-university1/cohort-4/CSSD-232.GA.T2.25.M2/SA/group-4/cis-api.git
cd cis-api
```
### Configure the database
You need to go into the Server library and into the appsettings file.
Switching between MongoDB and MySQL;
Replace the database provider with the database of your choice; it's important that it's written in this way:
```bash
"DatabaseProvider": "MongoDB",
```
Or:
```bash
"DatabaseProvider": "MySQL",
```
Note: The documents may not initially appear in MongoDB; you need to add the first content to structure the documents.

### Data Migration


To run the data migration tool locally, you need to configure the `appsettings.json` file of the `CisApi.DataMigration` project (or `MigrationTool`, depending on the exact structure) with the following connection strings:

```c#
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=127.0.0.1;Port=3307;Database=sd3;Uid=root;Pwd=sd5;",
    "MongoDb": "mongodb://localhost:27017"
  },
  // Other configurations...
}
```

Ensure that the MySQL server is accessible on port `3307` and MongoDB on port `27017`.



### Build the Solution

Navigate to the `backend-service` folder and build the solution. This will restore dependencies and compile the project.

```bash
cd backend-service
dotnet build CisApi.sln
```
### DataBase v3
#### 1.1 Start the MySQL container

From the User-API, start the Docker environment:

```bash 
docker-compose up -d mysql
```

This will launch the MySQL container and create the sd3 database (shared with the Users API).

#### 1.2 Start the MongoDB container

From the User-API, start the Docker environment:

```bash 
docker-compose up -d mongodb
```

This will launch the MongoDB container and create the test database (shared with the Users API).

#### 2. Create or update the MySQL database schema

If this is your first time running the project, you’ll need to apply the Entity Framework migrations to create the tables.

Navigate to the Infrastructure project:

```bash 
cd backend-service/CisApi.Infrastructure.MySQL
```

Then execute:

```bash 
dotnet ef database update
```

This will automatically create the required tables (such as topics and ideas) inside the existing sd3 database.

#### 3. Creating new migrations (for future updates)

If your branch includes model changes (e.g., new entities or columns), create a new migration before applying it:

```bash
dotnet ef migrations add <MigrationName>
dotnet ef database update
```

Example:

```bash 
dotnet ef migrations add AddIdeaVotes
dotnet ef database update
```

This ensures your database schema stays synchronized with the latest version of the domain models.

#### 4. Resetting the environment (optional)

If you need to start from scratch, stop and remove existing containers and volumes:

```bash 
docker compose down -v
docker compose up -d
dotnet ef database update
```

This will recreate the database and apply all migrations cleanly.

### Run the Project

After a successful build, you can run the `Server` project from the `backend-service/Server` folder:

```bash
cd backend-service/Server
dotnet run
```

This will start the API. The OpenAPI (Swagger) documentation will be available at `https://localhost:5020/swagger`.

### REST Tool 

#### Enviroment 

```bash
{
	"hostname": "localhost:5020",
	"base_url": "http://{{ hostname }}/api/v1",
	"auth_token": "Basic ZHVtbXk6ZHVtbXk=",
	"user_id": 1
}
```

#### Credentianls 

Password: dummy
Login: dummy

To Header: 
```bash 
Basic ZHVtbXk6ZHVtbXk=
```

### Dependencies

The project uses the following key dependencies:

*   **AutoMapper**: For object-to-object mapping.
*   **Entity Framework Core (with MySQL support)**: For database interaction.
*   **Mongo Drive (with MongoDb support)**: For database interaction.
*   **FluentValidation**: For building strong-typed validation rules.

### Testing and Code Coverage

The project includes xUnit tests and uses Coverlet for code coverage.

To run the tests and generate a code coverage report:

1.  Navigate to the test project directory:
    ```bash
    cd backend-service/Tests/Server.Tests
    ```
2.  Run the tests and collect code coverage:
    ```bash
    dotnet test --collect "XPlat Code Coverage"
    ```
    This will generate a `coverage.cobertura.xml` file in the `TestResults` folder.

3.  To generate an HTML report from the coverage data, ensure `ReportGenerator` is installed globally:
    ```bash
    dotnet tool install -g dotnet-reportgenerator-globaltool
    ```
4.  Generate the HTML report:
    ```bash
    reportgenerator "-reports:./TestResults/**/coverage.cobertura.xml" "-targetdir:./coverage-report" -reporttypes:Html
    ```
    The HTML report will be generated in the `coverage-report` directory. Open `coverage-report/index.html` in your browser to view the results.

### Continuous Integration/Continuous Deployment (CI/CD)

This project utilizes GitLab CI/CD for continuous integration and deployment. The pipeline is defined in the `.gitlab-ci.yml` file at the root of the repository.

The CI/CD pipeline includes the following stages:

*   **Build**: Compiles the backend service and builds the Docker image.
*   **Test**: Runs unit tests for the backend service.
*   **Deploy**: (Currently under development) Placeholder for future deployment logic.

The Docker image for the backend service is built and tagged with the commit short SHA.
