# 15-Troubleshooting

This section provides solutions to common issues you might encounter while developing, running, or interacting with the CIS API.

## General Issues

### Issue: API Fails to Start

*   **Symptom**: The `dotnet run` command (or Docker container) exits with an error, and the API is not accessible.
*   **Possible Causes**:
    *   Missing .NET SDK or incorrect version.
    *   Port conflict (another application is already using the required port, e.g., 5020).
    *   Configuration errors (e.g., invalid `appsettings.json` or environment variables).
    *   Build errors (check previous `dotnet build` output).
*   **Solution**:
    1.  **Check .NET SDK**: Ensure .NET 9.0 SDK is installed and correctly configured. Run `dotnet --version`.
    2.  **Check Port Availability**: If running directly, try changing the port in `Properties/launchSettings.json` or kill the process using the port. If using Docker, ensure the host port is free.
    3.  **Review Logs**: Examine the console output or Docker container logs for specific error messages. These often provide clues about the root cause.
    4.  **Clean and Rebuild**: Run `dotnet clean` followed by `dotnet build` in the `backend-service/Server` directory.

### Issue: Cannot Connect to the API

*   **Symptom**: Client applications or your browser cannot reach the API endpoint (e.g., `https://localhost:5020/swagger`).
*   **Possible Causes**:
    *   API is not running.
    *   Incorrect URL or port.
    *   Firewall blocking the connection.
    *   Docker container not running or port not mapped correctly.
*   **Solution**:
    1.  **Verify API Status**: Ensure the API is running (check console output or `docker ps`).
    2.  **Check URL/Port**: Double-check the URL and port you are trying to access.
    3.  **Firewall Settings**: Temporarily disable your firewall or add an exception for the API's port.
    4.  **Docker Issues**: If running in Docker, ensure the container is running (`docker ps`) and the port mapping is correct (`-p 5020:80`).

## Authentication and Authorization Issues

### Issue: `401 Unauthorized` Error

*   **Symptom**: Requests to protected endpoints return a `401 Unauthorized` status code.
*   **Possible Causes**:
    *   No authentication credentials provided.
    *   Invalid authentication credentials (e.g., incorrect username/password for Basic Auth).
    *   Expired or invalid token (if JWT is implemented in the future).
*   **Solution**:
    1.  **Provide Credentials**: Ensure your client application is sending the correct `Authorization` header.
    2.  **Verify Credentials**: Double-check the username and password used for Basic Authentication (e.g., `admin:admin`).
    3.  **Refer to Documentation**: Consult the [Authentication and Authorization](09-authentication-authorization.md) section for details on how to properly authenticate.

### Issue: `403 Forbidden` Error

*   **Symptom**: Requests to protected endpoints return a `403 Forbidden` status code.
*   **Possible Causes**:
    *   Authenticated user does not have the necessary permissions or role to perform the requested action.
*   **Solution**:
    1.  **Check User Permissions**: Verify that the authenticated user has the required role (e.g., `Admin`) for the specific operation.
    2.  **Review Authorization Logic**: If you are a developer, review the authorization logic in the API's code to understand the permission requirements for the endpoint.

## Database Connection Issues

### Issue: Cannot Connect to Database

*   **Symptom**: API logs show errors related to database connection (e.g., "Connection refused", "Access denied").
*   **Possible Causes**:
    *   Database server is not running.
    *   Incorrect database connection string in configuration.
    *   Incorrect credentials for database access.
    *   Firewall blocking database port.
*   **Solution**:
    1.  **Verify Database Status**: Ensure your MySQL (or MongoDB) server is running and accessible.
    2.  **Check Connection String**: Verify the database connection string in `appsettings.json` or environment variables.
    3.  **Check Credentials**: Ensure the username and password for the database are correct.
    4.  **Firewall**: Check if a firewall is blocking the database port.

## Build and Test Issues

### Issue: `dotnet build` or `dotnet test` Fails

*   **Symptom**: The build or test commands fail with compilation errors or test failures.
*   **Possible Causes**:
    *   Syntax errors in code.
    *   Missing dependencies.
    *   Incorrect project configuration.
    *   Test failures due to bugs in the code.
*   **Solution**:
    1.  **Examine Error Messages**: Carefully read the error messages in the console output. They usually pinpoint the exact location and nature of the problem.
    2.  **Restore Dependencies**: Run `dotnet restore` in the solution directory (`backend-service/`) to ensure all NuGet packages are downloaded.
    3.  **Clean and Rebuild**: Run `dotnet clean` followed by `dotnet build`.
    4.  **Debug Tests**: If tests are failing, use your IDE's debugger to step through the test code and the application code it calls.

For any issues not covered here, please refer to the project's issue tracker or seek assistance from the development team.