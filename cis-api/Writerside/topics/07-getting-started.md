# 07-Getting Started with CIS API

This guide will help you set up and run the CIS API locally, allowing you to explore its functionalities and begin development.

## Prerequisites

Before you begin, ensure you have the following installed on your system:

*   **.NET 9.0 SDK**: The CIS API is developed using C# and ASP.NET Core 9.0. You can download the SDK from the official .NET website.
    *   [Download .NET SDK](https://dotnet.microsoft.com/en-us/download)
*   **Docker (Optional, but Recommended)**: For running the API in a containerized environment, which provides consistency across different development setups.
    *   [Install Docker](https://www.docker.com/get-started)
*   **Git**: For cloning the project repository.
    *   [Install Git](https://git-scm.com/downloads)
*   **An IDE or Code Editor**: Such as Visual Studio Code or JetBrains Rider, with C# development extensions.

## Clone the Repository

To get a local copy of the project, clone the repository using the following command:

```bash
git clone https://gitlab.com/jala-university1/cohort-4/CSSD-232.GA.T2.25.M2/SA/group-4/cis-api.git
cd cis-api
```

## Run the API Locally

You have two primary ways to run the CIS API locally: directly from the source code or using Docker.

### Option 1: Run from Source Code

1.  **Navigate to the Server Project**:
    Open your terminal or command prompt and navigate to the `backend-service/Server` directory:
    ```bash
    cd backend-service/Server
    ```
2.  **Build the Project**:
    Restore dependencies and build the project:
    ```bash
    dotnet build
    ```
3.  **Run the API**:
    Start the API. This will typically launch the API on `https://localhost:5020` (or another port if configured differently in `launchSettings.json`):
    ```bash
    dotnet run
    ```
    The OpenAPI (Swagger) documentation will be available at `https://localhost:5020/swagger`.

### Option 2: Run using Docker

1.  **Build the Docker Image**:
    Navigate to the root of the `cis-api` project and build the Docker image. Ensure you are in the directory containing the `backend-service` folder.
    ```bash
    docker build -t cis-api-server -f backend-service/Server/Dockerfile backend-service/
    ```
    *(Note: `cis-api-server` is an example tag. You can choose any name.)*
2.  **Run the Docker Container**:
    Run the built image in a Docker container, mapping the API's port (e.g., 5020) to a port on your host machine:
    ```bash
    docker run -d -p 5020:80 --name cis-api-container cis-api-server
    ```
    This command runs the container in detached mode (`-d`), maps port `5020` on your host to port `80` inside the container, and names the container `cis-api-container`.
    The API will then be accessible at `http://localhost:5020`. The OpenAPI (Swagger) documentation will be at `http://localhost:5020/swagger`.

## API Endpoints Overview

The CIS API exposes several endpoints to manage topics, ideas, and votes. A detailed reference for each endpoint can be found in the [API Reference (Endpoints)](08-api-reference.md) section.

Here's a brief overview of the main resource categories:

*   `/api/v1/topics`: Endpoints for managing ideation topics (Create, Read, Update, Delete).
*   `/api/v1/ideas`: Endpoints for managing ideas within topics (Create, Read, Update, Delete).
*   `/api/v1/votes`: Endpoints for casting and managing votes on ideas.

Remember that most of these endpoints require authentication. Refer to the [Authentication and Authorization](09-authentication-authorization.md) section for details.