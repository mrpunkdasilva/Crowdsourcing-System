# 09-Authentication and Authorization

The CIS API leverages an external **Users API** for all authentication and authorization concerns. This design ensures a centralized and consistent approach to user management and security across different services within the Capstone Project.

## Integration with Users API

The CIS API does not manage user credentials directly. Instead, it acts as a client to the Users API to verify the identity and permissions of incoming requests.

*   **Users API Role**: The Users API is responsible for:
    *   Storing and managing user accounts.
    *   Authenticating users (e.g., validating credentials).
    *   Issuing authentication tokens (e.g., JWTs) or providing authentication status.

*   **CIS API Role**: The CIS API is responsible for:
    *   Receiving authentication information (e.g., tokens or credentials) from client requests.
    *   Forwarding or validating this information against the Users API.
    *   Enforcing authorization rules based on the authenticated user's identity and roles.

## Current Authentication Mechanism (for CIS API Integration)

For the purpose of integration and development of the CIS API, the authentication with the Users API is currently performed using **Basic Authentication**.

*   **Credentials**: The default credentials for testing and development are typically `admin` for both username and password.
    *   **Username**: `admin`
    **Password**: `admin`

*   **How it Works**: When a client makes a request to the CIS API that requires authentication, the CIS API will internally use these (or similar configured) Basic Authentication credentials to make a call to the Users API to validate the user's identity or obtain necessary authorization context.

*(Note: While the Users API's OpenAPI specification might indicate JWT as its primary security scheme, for the CIS API's internal integration with the Users API, Basic Authentication is currently employed. This may evolve in future phases.)*

## Roles and Permissions

The CIS API implements authorization rules based on the roles and permissions associated with the authenticated user. These roles are typically managed by the Users API and communicated to the CIS API during the authentication process.

*   **User Roles**: Specific roles (e.g., `Admin`, `Standard User`) define what actions a user can perform within the CIS API.
*   **Endpoint Protection**: Endpoints are protected based on the required permissions. For example, creating or deleting topics might require an `Admin` role, while voting on ideas might require a `Standard User` role.

## Example of Authenticated Request (Conceptual)

When making a request to a protected endpoint of the CIS API, the client would typically include an `Authorization` header. The CIS API then uses this information to interact with the Users API.

```http
POST /api/v1/topics
Authorization: Basic YWRtaW46YWRtaW4=  # Example for 'admin:admin' base64 encoded
Content-Type: application/json

{
  "title": "New Topic Title",
  "description": "Description of the new topic."
}
```
*(Note: The actual mechanism for the client to obtain and use tokens (e.g., JWT) from the Users API, and how the CIS API then validates these, will be detailed as the system evolves. The Basic Authentication example above is for the CIS API's internal interaction with the Users API during its current development phase.)*