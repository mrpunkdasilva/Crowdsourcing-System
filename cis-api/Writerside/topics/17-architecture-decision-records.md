# 17-Architecture Decision Records (ADRs)

This section documents the key architectural decisions made for the CIS API project. Each ADR provides context for the decision, the decision itself, and its consequences.

## ADR-001: Choice of .NET for the Backend API

*   **Status**: Accepted
*   **Date**: 2025-09-29
*   **Context**: The project requires a robust, scalable, and maintainable technology stack for the backend API. The development team has existing skills and the platform needs to integrate within an environment that may have other enterprise systems.
*   **Decision**: We will use ASP.NET Core with C# to develop the CIS API.
*   **Consequences**:
    *   **Pros**:
        *   High performance suitable for API workloads.
        *   Strongly-typed language (C#) reduces runtime errors.
        *   Large and mature ecosystem with extensive libraries (NuGet).
        *   Excellent tooling with Visual Studio and JetBrains Rider.
        *   The development team at Jala University has significant experience with the .NET stack, reducing ramp-up time.
    *   **Cons**:
        *   Can be perceived as more heavyweight than alternatives like Node.js or Python for very simple microservices.

## ADR-002: Use of OpenAPI/Swagger for API Definition

*   **Status**: Accepted
*   **Date**: 2025-09-29
*   **Context**: The CIS API will be consumed by various clients, including a validation client, and potentially a future web frontend. A clear, language-agnostic contract is essential for parallel development and clear communication.
*   **Decision**: We will adopt a "design-first" approach, defining the API using the OpenAPI 3.0 specification. The `Swashbuckle` library will be used to automatically generate a `swagger.json` file and expose the interactive Swagger UI.
*   **Consequences**:
    *   **Pros**:
        *   Provides an interactive documentation portal (Swagger UI) for easy exploration and testing of the API.
        *   Enables automatic generation of client SDKs in various languages.
        *   Serves as a single source of truth for the API's contract.
        *   Facilitates collaboration between backend and frontend teams.
    *   **Cons**:
        *   Requires discipline to keep the OpenAPI definition in sync with the implementation if not using code-first generation. (We mitigate this by using Swashbuckle which generates from code annotations).
