# Users API - Newman Tests

This directory contains the **Postman collection** and **environment** files used to test the Users API endpoints using **Newman**, the Postman command-line tool.

## Purpose

These automated tests validate the CRUD operations (Create, Read, Update, Delete) of the Users API.  
They ensure the API endpoints respond correctly and consistently with expected data and status codes.

## Requirements

To run these tests, you must have:

- **Node.js** (v18 or later)  
- **npm** (Node Package Manager)  
- **Newman** (Postman CLI tool)  
- **Newman HTML Reporter** (for generating HTML reports)

## Installation

Run the following commands to install the necessary dependencies globally:

```bash
npm install -g newman
npm install -g newman-reporter-html
```
## Running the Tests

Make sure your API server is running locally (default: http://localhost:8080).

Then execute the following command from the project directory:
```bash
newman run users-api-crud.postman_collection.json --environment local-api.postman_environment.json  --reporters cli,html  --reporter-html-export ./user_api_report.html
```

##  Output

**CLI Report:** Test results displayed in the terminal.

**HTML Report:** A detailed report will be generated as user_api_report.html in the current directory.

##  Files

*users-api-crud.postman_collection.json* Contains the API request definitions and test scripts.

*local-api.postman_environment.json* Contains the environment variables (e.g., API base URL, userId, etc.).
