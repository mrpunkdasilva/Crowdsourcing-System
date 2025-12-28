# 08-API Reference (Endpoints)

This section provides a comprehensive reference for all endpoints exposed by the CIS API. It details the available resources, HTTP methods, request parameters, and expected response formats.

## OpenAPI Specification

The CIS API's endpoints are formally defined using the OpenAPI Specification (OAS). This specification allows for automatic generation of interactive documentation, client SDKs, and server stubs, ensuring consistency and ease of use.

The full interactive API documentation, generated from the OpenAPI specification, can be accessed via the `/swagger` endpoint when the API is running locally (e.g., `https://localhost:5020/swagger`).

## General Endpoint Structure

All CIS API endpoints are versioned and follow a RESTful design. The base path for the API is typically `/api/v1/`.

### Resource Categories

The API is organized into the following main resource categories:

*   **Topics**: Manages ideation topics.
    *   **GET /api/v1/topics**: Retrieve a list of all topics.
    *   **GET /api/v1/topics/{id}**: Retrieve a specific topic by its ID.
    *   **POST /api/v1/topics**: Create a new topic.
    *   **PUT /api/v1/topics/{id}**: Update an existing topic.
    *   **DELETE /api/v1/topics/{id}**: Delete a specific topic.
*   **Ideas**: Manages ideas within topics.
    *   **GET /api/v1/topics/{topicId}/ideas**: Retrieve ideas for a specific topic.
    *   **GET /api/v1/ideas/{id}**: Retrieve a specific idea by its ID.
    *   **POST /api/v1/topics/{topicId}/ideas**: Create a new idea within a topic.
    *   **PUT /api/v1/ideas/{id}**: Update an existing idea.
    *   **DELETE /api/v1/ideas/{id}**: Delete a specific idea.
*   **Votes**: Manages votes on ideas.
    *   **POST /api/v1/ideas/{ideaId}/votes**: Cast a vote on an idea.
    *   **DELETE /api/v1/ideas/{ideaId}/votes/{voteId}**: Cancel a vote on an idea.

## Authentication

Most endpoints require authentication. Please refer to the [Authentication and Authorization](09-authentication-authorization.md) section for details on how to authenticate your requests.

## Error Responses

The API uses standard HTTP status codes to indicate the success or failure of an API request. Detailed error messages are provided in the response body. Refer to the [Error Handling](11-error-handling.md) section for more information on common error codes and their meanings.