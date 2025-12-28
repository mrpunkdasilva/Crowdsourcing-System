# About Users API

## 1. Database Configuration

The Users API can be configured to work with either **MySQL** or **MongoDB**. You can switch between them by setting the `spring.profiles.active` property in the `src/main/resources/application.properties` file.

*   **To use MySQL:**
    ```c#
    spring.profiles.active=mysql
    ```

*   **To use MongoDB:**
    ```c#
    spring.profiles.active=mongodb
    ```

The corresponding database connection details are located in `application-mysql.properties` and `application-mongodb.properties`.

**Note:** The MongoDB implementation currently throws `UnsupportedOperationException` for all data access methods. Full functionality will be added in a future update.

## 2. Getting Started (Local Development)

Follow these instructions to set up and run the Users API on your local machine.

#### 2.1 Prerequisites

Ensure you have the following installed:

*   **Java Development Kit (JDK) 17 or higher**: [Download JDK](https://www.oracle.com/java/technologies/downloads/)
*   **Apache Maven 3.8.x or higher**: [Download Maven](https://maven.apache.org/download.cgi)
*   **Docker and Docker Compose**: For running the database. [Install Docker](https://docs.docker.com/get-docker/)

#### 2.2 Build the Project

Navigate to the root directory of the project and build it using Maven:

```bash
mvn clean install
```

This command compiles the source code, runs tests, and packages the application into a JAR file.

#### 2.3 Run the Database

The project uses Docker Compose to manage the database.

1.  Navigate to the root directory of the project.
2.  Start the desired database container:
    *   **For MySQL:**
        ```bash
        docker-compose up -d mysql
        ```
    *   **For MongoDB:**
        ```bash
        docker-compose up -d mongodb
        ```
    This will start the database server in the background.

#### 2.4 Run the Spring Boot Application

After building the project and starting the database, you can run the Spring Boot application:

1.  **Using Maven (recommended for development):**
    ```bash
    mvn spring-boot:run
    ```
2.  **Using the Executable JAR:**
    ```bash
    java -jar target/users-api-0.0.1-SNAPSHOT.jar
    ```
    (Note: The exact JAR name might vary based on the project version in `pom.xml`.)

The application will start on `http://localhost:8080` by default.

#### 2.5 Access the API

Once the application is running, you can access the API endpoints.

*   **Health Check:**
    ```
    GET http://localhost:8080/api/v1/health
    ```
    This endpoint should return a `200 OK` status.

*   **Other Endpoints:** Refer to the "User Management Endpoints" section above for details on other available API endpoints. Remember that most endpoints require authentication.

## 3. Overview
The Users API is a **RESTful microservice developed in Java**. It manages user data (identifier, login, name, and password) stored in a **PostgreSQL database**. A key requirement is to maintain **data consistency** with an existing Java Command Line Interface (CLI) application that also interacts with the same user data. The API provides secure, authorized access for user operations.

## 4. Base Path
All API endpoints are prefixed with the base path: `/api/v1`

## 5. Authentication
Access to all endpoints requires authentication via JWT. Clients must include a valid JSON Web Token (JWT) in the `Authorization` header of their requests using the `Bearer` scheme.

**Example Request Header:**
```c#
Authorization: Bearer <YOUR_JWT_TOKEN_HERE>
```
A token must be obtained from a dedicated authentication endpoint (not defined in this specification).

## 6. User Management Endpoints
This section details the operations available for user data. All operations require prior authentication.

#### 6.1 Get a list of all users
*   **Description**: Retrieves a complete list of all registered users in the system. This endpoint is useful for administrative purposes or for displaying all users in a user management interface.
*   **Method**: `GET`
*   **Path**: `/api/v1/users`
*   **Responses**:
    *   `200 OK`: Refer to `UserListResponse` in [Data Models](#7-data-models).
    *   `400 Bad Request`: Refer to `BadRequestError` in [Data Models](#7-data-models).
    *   `404 Not Found`: Refer to `NotFoundError` in [Data Models](#7-data-models).
    *   `500 Internal Server Error`: Refer to `InternalServerError` in [Data Models](#7-data-models).

#### 6.2 Get a user by ID
*   **Description**: Retrieves a single user by their unique ID.
*   **Method**: `GET`
*   **Path**: `/api/v1/users/{id}`
*   **Parameters**:
    *   `id` (string): Unique user identifier.
*   **Responses**:
    *   `200 OK`: Refer to `UserResponseSuccess` in [Data Models](#7-data-models).
    *   `404 Not Found`: Refer to `NotFoundError` in [Data Models](#7-data-models).
    *   `500 Internal Server Error`: Refer to `InternalServerError` in [Data Models](#7-data-models).

#### 6.3 Create a new user
*   **Description**: Create a new user in the system
*   **Method**: `POST`
*   **Path**: `/api/v1/users`
*   **Request Body**: Refer to `UserCreateRequest` in [Data Models](#7-data-models).
*   **Responses**:
    *   `201 Created`: Refer to `UserResponseSuccess` in [Data Models](#7-data-models).
    *   `400 Bad Request`: Refer to `BadRequestError` in [Data Models](#7-data-models).
    *   `422 Unprocessable Entity`: Refer to `UnprocessableEntityError` in [Data Models](#7-data-models).
    *   `default`: Refer to `InternalServerError` in [Data Models](#7-data-models).

#### 6.4 Partially Update a User
*   **Description**: Updates one or more fields for a specific user.
*   **Method**: `PATCH`
*   **Path**: `/api/v1/users/{id}`
*   **Parameters**:
    *   `id` (string): Unique user identifier.
*   **Request Body**: Refer to `UserUpdateRequest` in [Data Models](#7-data-models).
*   **Responses**:
    *   `200 OK`: Refer to `UserResponseSuccess` in [Data Models](#7-data-models).
    *   `400 Bad Request`: Refer to `BadRequestError` in [Data Models](#7-data-models).
    *   `404 Not Found`: Refer to `NotFoundError` in [Data Models](#7-data-models).
    *   `default`: Refer to `InternalServerError` in [Data Models](#7-data-models).

#### 6.5 Delete a User
*   **Description**: Permanently deletes a user from the system based on their unique `id`. This action is irreversible. Ensure the user `id` is correct before making the request.
*   **Method**: `DELETE`
*   **Path**: `/api/v1/users/{id}`
*   **Parameters**:
    *   `id` (string): Unique user identifier.
*   **Responses**:
    *   `204 No Content`: Refer to `NoContentResponse` in [Data Models](#7-data-models).
    *   `404 Not Found`: Refer to `NotFoundError` in [Data Models](#7-data-models).

## 7. Data Models
The API uses the following data models for requests and responses.

#### 7.1 User Update Request Object (`UserUpdateRequest`)
This object is used in the request body when updating a user. All fields are optional.

| Field    | Type        | Description                                  | Validation      |
| :------- | :---------- | :------------------------------------------- | :-------------- |
| `name`   | `string`    | The full name of the user.                   | `minLength: 2`  |
| `login`  | `string`    | The **unique** username used for login.      | `minLength: 3`  |
| `password` | `string`    | The user\'s new password.                     | `minLength: 8`  |

#### 7.2 User Create Request Object (`UserCreateRequest`)
This object is used in the request body when creating a new user.

| Field    | Type        | Description                                  |
| :------- | :---------- | :------------------------------------------- |
| `name`   | `string`    | The full name of the user.                   |
| `login`  | `string`    | The **unique** username used for login.      |
| `password` | `string`    | The user\'s password.                         |

#### 7.3 User Response Object (`UserResponse`)
This object is returned when user information is requested.

| Field    | Type        | Description                                  |
| :------- | :---------- | :------------------------------------------- |
| `id`     | `string`    | **Unique identifier** for the user.          |
| `name`   | `string`    | The full name of the user.                   |
| `login`  | `string`    | The **unique** username used for login.      |

#### 7.4 Error Object (`Error`)
This object is returned for all types of errors, providing a consistent structure for error reporting.

| Field    | Type        | Description                                  |
| :------- | :---------- | :------------------------------------------- |
| `code`   | `string`    | A machine-readable error code (e.g., `BAD_REQUEST`, `NOT_FOUND`, `INTERNAL_SERVER_ERROR`, `UNPROCESSABLE_ENTITY`). |
| `message`  | `string`    | A human-readable message describing the error. |

## 8. API Design Standard
This API is formally defined using the **OpenAPI Specification**. This practice ensures clarity, facilitates communication between development teams, and enables the automatic generation of interactive documentation.

## 9. Code Style and Quality
To maintain a high standard of code quality and consistency, this project uses **Checkstyle**. The configuration is based on a custom style guide tailored for this project, enforcing rules for indentation, brace style, Javadoc, and more.

All contributions must adhere to these coding standards. The build process automatically runs Checkstyle to validate the code.

For a detailed guide on the enforced rules and how to run checks manually, please refer to the Checkstyle Configuration Documentation.