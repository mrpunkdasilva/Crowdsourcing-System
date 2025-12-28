# Local development setup

This guide provides step-by-step instructions to set up your local development environment for the **Users API**.

## Prerequisites

Before you begin, ensure you have the following tools installed:

*   **Git**: For cloning the repository.
*   **Java Development Kit (JDK)**: Version 17.
*   **Apache Maven**: Version 3.6 or later.
*   **Docker**: To run the MySQL database in a container.
*   **An IDE**: We recommend [IntelliJ IDEA](https://www.jetbrains.com/idea/) for the best experience.

## 1. Clone the Repository

Open your terminal and clone the project repository from GitHub.

```bash
git clone https://github.com/mrpunkdasilva/capstone-ds3.git
cd capstone-ds3/users-api
```

## 2. Start the Database with Docker

The project can be configured to work with either a **MySQL** or **MongoDB** database running in Docker containers. A `compose.yaml` file is provided at the root of the `users-api` directory to make this easy.

In your terminal, from the `users-api` directory, run one of the following commands:

*   **To start MySQL:**
    ```bash
    docker-compose up -d mysql
    ```
    This will start a MySQL container in detached mode (`-d`). The database will be available on its default port, and the necessary schema will be initialized from the script at `docker/mysql/initdb/init.sql`.

*   **To start MongoDB:**
    ```bash
    docker-compose up -d mongodb
    ```
    This will start a MongoDB container in detached mode (`-d`).

**Note:** Ensure only one database container is running at a time if you intend to use a single profile for the application.

## 3. Build the Application

Use Maven to build the project. This command will compile the source code, run tests, and download all required dependencies.

```bash
mvn clean install
```

## 4. Run the Application

Once the build is complete, you can run the application using the Spring Boot Maven plugin, specifying the active profile for your chosen database:

*   **To run with MySQL (default profile):**
    ```bash
    mvn spring-boot:run
    ```
    or
    ```bash
    java -jar target/users-api-0.0.1-SNAPSHOT.jar
    ```

*   **To run with MongoDB:**
    ```bash
    mvn spring-boot:run -Dspring.profiles.active=mongodb
    ```
    or
    ```bash
    java -jar -Dspring.profiles.active=mongodb target/users-api-0.0.1-SNAPSHOT.jar
    ```

The API will start and be accessible at `http://localhost:8080`.

## 5. Verify the Setup

To confirm that the application is running correctly and connected to the database, you can access the health check endpoint.

Open your browser or use a tool like `curl`:

```bash
curl http://localhost:8080/health
```

You should receive a response indicating the status is `UP`:

```json
{
  "status": "UP"
}
```

Your local development environment is now ready! You can start making changes to the code.