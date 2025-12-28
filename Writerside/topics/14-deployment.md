# 14-Deployment

Deployment is the process of making the CIS API available for use in various environments (e.g., development, staging, production). This section outlines the conceptual deployment strategy, primarily focusing on containerization with Docker.

## Deployment Strategy Overview

The CIS API is designed to be deployed as a Docker container, which provides a consistent and isolated environment for the application. This approach simplifies dependency management and ensures that the API runs reliably across different infrastructures.

The current CI/CD pipeline (refer to [Continuous Integration/Continuous Deployment (CI/CD)](13-ci-cd.md)) is configured to build a Docker image of the backend service. This image serves as the deployable artifact.

## Deployable Artifact: Docker Image

After a successful CI/CD pipeline run, a Docker image for the CIS API backend service is built and tagged. This image contains:

*   The compiled CIS API application.
*   All necessary runtime dependencies.
*   A configured environment to run the application.

The image is typically tagged with the short SHA of the Git commit that triggered the build, ensuring traceability.

## Conceptual Deployment Process

While the automated deployment logic is currently a placeholder in the CI/CD pipeline, the general steps for deploying the Docker image would involve:

1.  **Pushing the Docker Image to a Registry**:
    The built Docker image would be pushed to a container registry (e.g., GitLab Container Registry, Docker Hub, or a private registry). This makes the image accessible from your deployment environment.
    *(Note: The `docker_build_backend` job currently builds the image but does not push it. This step would be automated in a fully implemented CD pipeline.)*

2.  **Pulling the Image in the Target Environment**:
    On the target server or container orchestration platform (e.g., Kubernetes, Docker Swarm), the Docker image would be pulled from the registry.
    ```bash
    docker pull registry.gitlab.com/mrpunkdasilva/capstone-ds3/cis-api/backend-service/server:<CI_COMMIT_SHORT_SHA>
    ```
    *(Replace `<CI_COMMIT_SHORT_SHA>` with the actual commit SHA or a specific version tag.)*

3.  **Running the Container**:
    The Docker image would then be run as a container, exposing the API's port and configuring any necessary environment variables.
    ```bash
    docker run -d -p 8080:80 --name cis-api-production-instance -e "ASPNETCORE_ENVIRONMENT=Production" registry.gitlab.com/mrpunkdasilva/capstone-ds3/cis-api/backend-service/server:<CI_COMMIT_SHORT_SHA>
    ```
    *(This example maps host port 8080 to container port 80, sets the environment to Production, and names the container.)*

## Environment Variables and Configuration

The CIS API relies on environment variables for configuration, especially in different deployment environments. Key configuration aspects include:

*   **Database Connection Strings**: For connecting to the MySQL (or later MongoDB) database.
*   **Users API Endpoint**: The URL of the external Users API for authentication.
*   **Logging Levels**: Configuration for application logging.
*   **Security Settings**: Any secrets or keys required for secure operation.

These variables should be securely managed in the deployment environment and injected into the Docker container at runtime.

## Future Enhancements

Future development will focus on automating the deployment process, potentially integrating with container orchestration tools and implementing blue/green or canary deployment strategies for zero-downtime updates.