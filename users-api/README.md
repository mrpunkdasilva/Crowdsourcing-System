# **Users API**

This document provides a comprehensive overview of the **Users API**, a RESTful microservice for user management. It covers everything from local setup to API endpoint details, ensuring that junior developers can get started quickly.

## **Table of Contents**
1.  [Overview](#1-overview)
2.  [Getting Started (Local Development)](#2-getting-started-local-development)
    *   [Prerequisites](#21-prerequisites)
    *   [Build and Test](#22-build-and-test)
    *   [Run the Database](#23-run-the-database)
    *   [Run the Application](#24-run-the-application)
    *   [Data Migration (MySQL to MongoDB)](#25-data-migration-mysql-to-mongodb)
3.  [API Endpoints](#3-api-endpoints)
    *   [Base Path](#31-base-path)
    *   [Authentication](#32-authentication)
    *   [Health Check](#33-health-check)
    *   [Authenticate User](#34-authenticate-user)
    *   [Get All Users](#35-get-all-users)
    *   [Get User by ID](#36-get-user-by-id)
    *   [Create User](#37-create-user)
    *   [Update User (Partial)](#38-update-user-partial)
    *   [Delete User](#39-delete-user)
4.  [Data Models (DTOs)](#4-data-models-dtos)
    *   [`UserCreateDto`](#41-usercreatedto)
    *   [`UserUpdateDto`](#42-userupdatedto)
    *   [`UserResponseDto`](#43-userresponsedto)
    *   [Error Handling](#44-error-handling)
5.  [Database Configuration](#5-database-configuration)
6.  [Code Style and Quality](#6-code-style-and-quality)
7.  [API Design Standard](#7-api-design-standard)

---

## **1. Overview**
The Users API is a **RESTful microservice built with Java and Spring Boot**. It provides a secure interface for managing user data, including creating, reading, updating, and deleting users.

A key feature is its ability to work with different databases (**MySQL** or **MongoDB**) through configuration, making it adaptable to various environments.

## **2. Getting Started (Local Development)**

Follow these instructions to set up and run the API on your local machine.

#### **2.1 Prerequisites**

*   **JDK 17** (or higher)
*   **Maven 3.8.x** (or higher)
*   **Docker & Docker Compose**

#### **2.2 Build and Test**

Clone the repository and run the following Maven command to compile the code, run all tests, and package the application:

```bash
./mvn clean install
```

This ensures the application is working correctly before you run it.

#### **2.3 Run the Database**

The project uses Docker Compose to manage the database services. You can start individual services or all of them:

*   **Start MySQL only:**
    ```bash
    docker compose up -d mysql
    ```
*   **Start MongoDB only:**
    ```bash
    docker compose up -d mongodb
    ```
*   **Start both MySQL and MongoDB:**
    ```bash
    docker compose up -d
    ```

#### **2.4 Run the Application**

The application can be run with different Spring profiles to connect to either MySQL or MongoDB.

*   **Run with MongoDB (Default):**
    ```bash
    ./mvn spring-boot:run
    ```
    This will start the application connected to MongoDB.

*   **Run with MySQL:**
    ```bash
    ./mvn spring-boot:run -Pmysql
    ```
    This will start the application connected to MySQL.

#### **2.5 Data Migration (MySQL to MongoDB)**

The Users API includes a data migration feature to transfer user data from a MySQL database to a MongoDB database. This process is managed via a dedicated Spring profile.

**How it works:**
When the `migration` profile is active, the application will:
1.  Connect to both MySQL and MongoDB.
2.  Drop the existing `users` collection in MongoDB (to prevent data duplication).
3.  Read all user data from the MySQL `users` table.
4.  Map and insert this data into the MongoDB `users` collection.
5.  Terminate gracefully after the migration is complete (the web server is disabled for this profile).

**To run the migration:**

1.  Ensure both your MySQL and MongoDB Docker containers are running (see [2.3 Run the Database](#23-run-the-database)).
2.  Execute the following command:
    ```bash
    ./mvn spring-boot:run -Pmigration
    ```
    You will see log messages indicating the progress and completion of the migration.

## **3. API Endpoints**

This section details the available API endpoints.

#### **3.1 Base Path**

All API endpoints are prefixed with: `/api/v1`

#### **3.2 Authentication**

All endpoints, except for `/health`, require **HTTP Basic Authentication**.

Use the credentials `admin` for the username and `admin` for the password.

**Example with `curl`:**

```bash
curl -u admin:admin http://localhost:8080/api/v1/users
```

The `-u` flag handles the Base64 encoding of your credentials and adds the `Authorization` header automatically.

#### **3.3 Health Check**

*   **Description**: Checks if the API is running. No authentication is required.
*   **Endpoint**: `GET /health`
*   **Success Response**: `200 OK`

#### **3.4 Authenticate User**

*   **Description**: Authenticates a user with their credentials and returns their data if successful.
*   **Endpoint**: `POST /auth`
*   **Success Response**: `200 OK` with a `UserResponseDto` object.
*   **Error Response**: `404 Not Found` if the credentials are not valid.

**Authentication Flow:**

1.  **Create a User**: If you don't have a user, create one by sending a `POST` request to `/api/v1/users`. Note the `login` and `password` you used.

2.  **Encode Credentials**: Manually encode the `login` and `password` in the format `login:password` using Base64. You can use an online tool like [base64encode.org](https://www.base64encode.org/).
    *   For example, if your login is `testuser` and password is `password123`, you would encode the string `testuser:password123`.

3.  **Send Request**: Send a `POST` request to `/api/v1/auth` with an `Authorization` header. The header value must be `Basic ` followed by your Base64 encoded string.

    **Example Header:**
    ```
    Authorization: Basic dGVzdHVzZXI6cGFzc3dvcmQxMjM=
    ```

#### **3.5 Get All Users**

*   **Description**: Retrieves a list of all users.
*   **Endpoint**: `GET /users`
*   **Success Response**: `200 OK` with a list of `UserResponseDto` objects.

#### **3.6 Get User by ID**

*   **Description**: Retrieves a single user by their unique ID.
*   **Endpoint**: `GET /users/{id}`
*   **Success Response**: `200 OK` with a `UserResponseDto` object.
*   **Error Response**: `404 Not Found` if the ID does not exist.

#### **3.7 Create User**

*   **Description**: Creates a new user.
*   **Endpoint**: `POST /users`
*   **Request Body**: `UserCreateDto`
*   **Success Response**: `201 Created` with the created `UserResponseDto`.
*   **Error Response**: `400 Bad Request` for validation errors.

#### **3.8 Update User (Partial)**

*   **Description**: Updates one or more fields for an existing user.
*   **Endpoint**: `PATCH /users/{id}`
*   **Request Body**: `UserUpdateDto`
*   **Success Response**: `200 OK` with the updated `UserResponseDto`.
*   **Error Responses**:
    *   `400 Bad Request` for validation errors.
    *   `404 Not Found` if the ID does not exist.

#### **3.9 Delete User**

*   **Description**: Deletes a user by their unique ID.
*   **Endpoint**: `DELETE /users/{id}`
*   **Success Response**: `200 OK`.
*   **Error Response**: `404 Not Found` if the ID does not exist.

## **4. Data Models (DTOs)**

The API uses Data Transfer Objects (DTOs) for requests and responses.

#### **4.1 `UserCreateDto`**

Used in the request body to create a new user. All fields are required.

| Field      | Type     | Description                        |
| :--------- | :------- | :--------------------------------- |
| `name`     | `string` | The full name of the user.         |
| `login`    | `string` | The unique username for login.     |
| `password` | `string` | The user's password.               |

#### **4.2 `UserUpdateDto`**

Used in the request body to update a user. All fields are optional.

| Field      | Type     | Description               | Validation       |
| :--------- | :------- | :------------------------ | :--------------- |
| `name`     | `string` | The new full name.        | `minLength: 2`   |
| `login`    | `string` | The new unique username.  | `minLength: 3`   |
| `password` | `string` | The new password.         | `minLength: 8`   |

#### **4.3 `UserResponseDto`**

The standard object returned for successful user operations.

| Field   | Type     | Description                   |
| :------ | :------- | :---------------------------- |
| `id`    | `string` | The unique user identifier.   |
| `name`  | `string` | The full name of the user.    |
| `login` | `string` | The unique username.          |

#### **4.4 Error Handling**

In case of an error (e.g., validation failure, resource not found), the API returns a standard JSON object:

| Field     | Type     | Description                                  |
| :-------- | :------- | :------------------------------------------- |
| `code`    | `string` | A machine-readable error code (e.g., `BAD_REQUEST`). |
| `message` | `string` | A human-readable message describing the error. |

## **5. Database Configuration**

The API can be configured to use either **MySQL** or **MongoDB**. To switch, use the appropriate Spring profile when running the application.

*   **MongoDB Profile (Default):**
    *   Activates MongoDB as the primary data source.
    *   Configuration in `src/main/resources/application-mongodb.properties`.
    *   Activated by default or with `./mvn spring-boot:run -Pmongodb`.

*   **MySQL Profile:**
    *   Activates MySQL as the primary data source.
    *   Configuration in `src/main/resources/application-mysql.properties`.
    *   Activated with `./mvn spring-boot:run -Pmysql`.

*   **Migration Profile:**
    *   Activates both MySQL and MongoDB configurations for data transfer.
    *   Configuration in `src/main/resources/application-migration.properties`.
    *   Activated with `./mvn spring-boot:run -Pmigration`.

## **6. Code Style and Quality**

This project uses **Checkstyle** to enforce a consistent code style. The build process (`mvn clean install`) automatically runs Checkstyle. For more details, see the [Checkstyle Configuration Documentation](./config/checkstyle/README.md).

## **7. API Design Standard**

The API is formally defined using the **OpenAPI Specification**. The definition can be found in `swagger/openapi.yaml`. This enables auto-generating interactive documentation and ensures clear communication.
