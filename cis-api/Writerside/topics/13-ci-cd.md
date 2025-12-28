# 13-Continuous Integration/Continuous Deployment (CI/CD)

Continuous Integration (CI) and Continuous Deployment (CD) are essential practices for modern software development, enabling rapid and reliable delivery of software. The CIS API project leverages **GitLab CI/CD** to automate the build, test, and packaging processes.

## GitLab CI/CD Pipeline Overview

The CI/CD pipeline for the CIS API is defined in the `.gitlab-ci.yml` file located at the root of the repository. This file orchestrates a series of jobs that are executed automatically upon code changes (e.g., pushes to the repository).

The pipeline is structured into distinct stages, ensuring a systematic flow from code commit to a deployable artifact.

### Pipeline Stages

The `.gitlab-ci.yml` defines the following stages:

*   **`build`**: This stage is responsible for compiling the application and preparing any necessary artifacts.
*   **`test`**: This stage executes automated tests to verify the correctness and quality of the code.
*   **`deploy`**: This stage handles the deployment of the application. (Note: This stage is currently under development and primarily focuses on building the Docker image.)

## Backend Service CI/CD Jobs

The following jobs are specifically configured for the CIS API backend service:

### `build_backend` Job

*   **Stage**: `build`
*   **Purpose**: Restores NuGet packages and builds the .NET solution.
*   **Image**: `mcr.microsoft.com/dotnet/sdk:9.0`
*   **Script**:
    ```bash
    dotnet restore backend-service/CisApi.sln
    dotnet build backend-service/CisApi.sln --configuration Release
    ```
*   **Artifacts**: Stores the compiled output of the backend service.

### `test_backend` Job

*   **Stage**: `test`
*   **Purpose**: Executes the automated tests for the backend service.
*   **Image**: `mcr.microsoft.com/dotnet/sdk:9.0`
*   **Script**:
    ```bash
    dotnet test backend-service/CisApi.sln --configuration Release
    ```
*   **Dependencies**: Depends on the successful completion of the `build_backend` job.

### `docker_build_backend` Job

*   **Stage**: `deploy`
*   **Purpose**: Builds the Docker image for the CIS API backend service.
*   **Image**: `docker:latest`
*   **Services**: Utilizes `docker:dind` (Docker-in-Docker) to enable Docker commands within the job.
*   **Script**:
    ```bash
    docker build -t registry.gitlab.com/mrpunkdasilva/capstone-ds3/cis-api/backend-service/server:$CI_COMMIT_SHORT_SHA -f backend-service/Server/Dockerfile backend-service/
    ```
    *(Note: This job currently only builds the Docker image and does not push it to a registry due to configuration. Future enhancements may include pushing to a container registry.)*
*   **Dependencies**: Depends on the successful completion of the `test_backend` job.

## How to Trigger the Pipeline

The GitLab CI/CD pipeline is automatically triggered by various Git events, such as:

*   **Pushes to branches**: Any `git push` to the repository will trigger a pipeline run.
*   **Merge Requests**: Opening or updating a merge request will also trigger a pipeline, ensuring that changes are validated before merging.

By automating these steps, GitLab CI/CD helps maintain code quality, speeds up the development cycle, and ensures that the CIS API is always in a releasable state.