# 06-Architecture Overview

This section provides a high-level overview of the CIS API's architecture. For a more detailed visual representation, refer to the [C4 Model for Software Architecture](18-c4-model.md). Key architectural decisions are documented in our [Architecture Decision Records (ADRs)](17-architecture-decision-records.md).

The API is designed to be modular, scalable, and maintainable, following best practices in modern software development. It's composed of the following main components:

## Architectural Style

The CIS API is developed following a **Microservices Architecture (MSA)**. This approach was chosen to enable independent development, deployment, and scalability of different parts of the system. Each microservice is responsible for a specific business capability, promoting a clear **Separation of Concerns (SoC)**.

Within the CIS API itself, principles of **Hexagonal Architecture (Ports and Adapters)** are applied. This pattern promotes modularity and testability by isolating the core business logic from external dependencies (such as UI frameworks, databases, and external APIs). This isolation is particularly beneficial for planned database migrations.

## Key Components

The CIS API interacts with several key components within the Capstone Project ecosystem:

*   **User API (Java)**: A RESTful microservice developed in Java, responsible for user management (CRUD operations) and authentication. The CIS API relies on the User API for authenticating requests.
*   **CIS API (C#)**: The core of the ideation platform, developed in C# ASP.NET Core. It manages topics, ideas, and votes. It depends on the User API for authentication.
*   **Database**: The system's persistence layer. Initially, a relational database (MySQL) is used, which will evolve to a NoSQL database (MongoDB) in later phases. The CIS API interacts with this database to store and retrieve ideation data.
*   **Legacy System (CLI Java)**: An existing command-line application that manages users. The new system (including the CIS API) is designed to coexist and maintain data consistency with this component.
*   **App Client**: A simple client application (simulator) that interacts with the APIs to validate end-to-end system functionality.

