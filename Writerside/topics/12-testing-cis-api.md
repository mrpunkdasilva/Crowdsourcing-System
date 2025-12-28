# 12-Testing the CIS API

Thorough testing is a critical aspect of developing a high-quality and reliable API. This section outlines the testing strategies and tools used for the CIS API, covering both automated tests and manual API client testing.

## Automated Testing

The CIS API project includes automated tests to ensure the correctness of its functionalities and to prevent regressions. These tests are primarily written using **xUnit** and cover various levels of testing.

### Running Unit and Integration Tests

1.  **Navigate to the Test Project Directory**:
    Open your terminal or command prompt and navigate to the test project directory:
    ```bash
    cd backend-service/Tests/Server.Tests
    ```
2.  **Run Tests**:
    Execute all tests within the project:
    ```bash
    dotnet test
    ```
    This command will compile and run all unit and integration tests, providing a summary of passed and failed tests.

### Code Coverage

Code coverage is measured using **Coverlet** to ensure that a significant portion of the codebase is exercised by tests.

1.  **Run Tests and Collect Code Coverage**:
    To run tests and generate a code coverage report in Cobertura XML format:
    ```bash
    dotnet test --collect "XPlat Code Coverage"
    ```
    This will generate a `coverage.cobertura.xml` file in a `TestResults` subfolder (e.g., `backend-service/Tests/Server.Tests/TestResults/{GUID}/coverage.cobertura.xml`).

2.  **Generate HTML Report (Optional)**:
    To visualize the code coverage data in an easily readable HTML format, you can use **ReportGenerator**.
    *   **Install ReportGenerator (if not already installed)**:
        ```bash
        dotnet tool install -g dotnet-reportgenerator-globaltool
        ```
    *   **Generate the HTML Report**:
        Navigate to the `backend-service/Tests/Server.Tests` directory and run:
        ```bash
        reportgenerator "-reports:./TestResults/**/coverage.cobertura.xml" "-targetdir:./coverage-report" -reporttypes:Html
        ```
        The HTML report will be generated in the `./coverage-report` directory. Open `coverage-report/index.html` in your browser to view the results.

## Manual API Testing with Clients

Beyond automated tests, API clients are invaluable for manual testing, exploring endpoints, and debugging interactions.

### Recommended API Clients

*   **Postman**: A comprehensive platform for API development, allowing you to design, test, and document APIs.
    *   [Official Website](https://www.postman.com/)
*   **Insomnia**: An open-source API client with a clean interface, focusing on REST, GraphQL, and gRPC.
    *   [Official Website](https://insomnia.rest/)
*   **Bruno**: A fast, open-source, and Git-friendly API client that stores collections directly in your filesystem.
    *   [Official Website](https://www.usebruno.com/)

### Example Test Cases (using an API Client)

Here are some basic test cases you can perform using any of the recommended API clients:

1.  **Create a Topic (POST /api/v1/topics)**:
    *   **Request**: Send a POST request to `/api/v1/topics` with a JSON body containing `title` and `description`.
    *   **Expected**: `201 Created` status code and the newly created topic object in the response.
    *   **Authentication**: Requires authentication (refer to [Authentication and Authorization](09-authentication-authorization.md)).

2.  **Get All Topics (GET /api/v1/topics)**:
    *   **Request**: Send a GET request to `/api/v1/topics`.
    *   **Expected**: `200 OK` status code and a list of topic objects.
    *   **Authentication**: May or may not require authentication, depending on configuration.

3.  **Create an Idea (POST /api/v1/topics/{topicId}/ideas)**:
    *   **Request**: Send a POST request to `/api/v1/topics/{topicId}/ideas` with a JSON body containing `title` and `description`. Replace `{topicId}` with an existing topic's ID.
    *   **Expected**: `201 Created` status code and the newly created idea object.
    *   **Authentication**: Requires authentication.

4.  **Vote on an Idea (POST /api/v1/ideas/{ideaId}/votes)**:
    *   **Request**: Send a POST request to `/api/v1/ideas/{ideaId}/votes` with a JSON body specifying the `type` of vote (e.g., `"upvote"`).
    *   **Expected**: `201 Created` status code and the vote confirmation.
    *   **Authentication**: Requires authentication.

Remember to consult the [API Reference (Endpoints)](08-api-reference.md) for detailed request and response structures for each endpoint.