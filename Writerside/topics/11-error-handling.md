# 11-Error Handling

Effective error handling is crucial for building robust and user-friendly APIs. The CIS API follows standard HTTP status codes and provides consistent error response structures to help clients understand and resolve issues.

## General Error Structure

When an error occurs, the API typically returns a JSON object with a `code` and a `message` field.

| Field     | Type     | Description                                     | Example Value                                |
| :-------- | :------- | :---------------------------------------------- | :------------------------------------------- |
| `code`    | `string` | A unique, machine-readable error code.          | `"BAD_REQUEST"`                              |
| `message` | `string` | A human-readable description of the error.      | `"Invalid input provided."`                  |

**Example JSON (Error Response):**

```json
{
  "code": "BAD_REQUEST",
  "message": "Invalid input provided. The 'title' field is required."
}
```

## Common HTTP Status Codes and Error Types

The CIS API utilizes standard HTTP status codes to indicate the nature of the response. Here are some common codes you might encounter:

*   **`2xx` Success**:
    *   `200 OK`: The request was successful.
    *   `201 Created`: A new resource was successfully created.
    *   `204 No Content`: The request was successful, but there is no content to return (e.g., for a successful DELETE operation).

*   **`4xx` Client Errors**: These indicate that the client's request was somehow incorrect or invalid.

    *   **`400 Bad Request`**:
        *   **Description**: The server cannot process the request due to a client error (e.g., malformed request syntax, invalid request message framing, or deceptive request routing). This often occurs due to invalid JSON format or missing required fields.
        *   **Error Code Example**: `"BAD_REQUEST"`
        *   **Example Scenario**: Sending a POST request with a non-JSON body or a JSON body missing a mandatory field.

    *   **`401 Unauthorized`**:
        *   **Description**: The request has not been applied because it lacks valid authentication credentials for the target resource. This means the user is not authenticated.
        *   **Error Code Example**: `"UNAUTHORIZED"`
        *   **Example Scenario**: Attempting to access a protected endpoint without providing any authentication credentials or with invalid ones.

    *   **`403 Forbidden`**:
        *   **Description**: The server understood the request but refuses to authorize it. This means the user is authenticated but does not have the necessary permissions to access the resource or perform the action.
        *   **Error Code Example**: `"FORBIDDEN"`
        *   **Example Scenario**: An authenticated "Standard User" attempting to delete a topic, which might be an "Admin" only operation.

    *   **`404 Not Found`**:
        *   **Description**: The server cannot find the requested resource. This often occurs when trying to access a resource with an ID that does not exist.
        *   **Error Code Example**: `"NOT_FOUND"`
        *   **Example Scenario**: Requesting a topic with an ID that is not present in the database.

    *   **`409 Conflict`**:
        *   **Description**: The request could not be completed due to a conflict with the current state of the target resource. This can happen, for example, if you try to create a resource that already exists with a unique identifier.
        *   **Error Code Example**: `"CONFLICT"`
        *   **Example Scenario**: Attempting to create a topic with a title that must be unique and already exists.

    *   **`442 Unprocessable Entity`**:
        *   **Description**: The server understands the content type of the request entity, and the syntax of the request entity is correct, but it was unable to process the contained instructions. This often indicates semantic errors in the request body (e.g., invalid data values, business rule violations).
        *   **Error Code Example**: `"VALIDATION_ERROR"`
        *   **Example Scenario**: Sending a request to create an idea with a `topicId` that is not a valid UUID format.

*   **`5xx` Server Errors**: These indicate that the server failed to fulfill a request.

    *   **`500 Internal Server Error`**:
        *   **Description**: A generic error message, given when an unexpected condition was encountered and no more specific message is suitable. This usually indicates an unhandled exception on the server side.
        *   **Error Code Example**: `"INTERNAL_SERVER_ERROR"`
        *   **Example Scenario**: An unexpected database connection issue or a bug in the API's business logic.

## Handling Errors in Client Applications

When consuming the CIS API, client applications should:

1.  **Check HTTP Status Codes**: Always inspect the HTTP status code of the response to quickly determine if the request was successful or if an error occurred.
2.  **Parse Error Body**: For `4xx` and `5xx` responses, parse the JSON response body to extract the `code` and `message` fields for more specific error information.
3.  **Implement Retry Logic**: For transient `5xx` errors (e.g., `503 Service Unavailable`), consider implementing retry mechanisms with exponential backoff.
4.  **Log Errors**: Log detailed error information on the client side for debugging and monitoring.

By following these guidelines, client applications can effectively handle errors from the CIS API, leading to a more robust and reliable integration.