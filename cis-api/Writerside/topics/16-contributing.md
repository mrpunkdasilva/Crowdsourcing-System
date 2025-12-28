# 16-Contributing to CIS API

We welcome contributions to the CIS API project! By following these guidelines, you can help ensure a smooth and collaborative development process, maintaining code quality and consistency.

## Code Style Guidelines

Adhering to a consistent code style is crucial for readability and maintainability. For the CIS API, we follow standard C# and .NET coding conventions.

*   **C# Coding Conventions**:
    *   Follow Microsoft's official C# coding conventions.
    *   Use meaningful names for variables, methods, and classes.
    *   Prefer `var` when the type is obvious from the right-hand side.
    *   Use `async` and `await` for asynchronous operations.
    *   Organize `using` directives alphabetically.
*   **.NET Best Practices**:
    *   Adhere to .NET design guidelines for API development.
    *   Ensure proper error handling and logging.
    *   Write unit tests for new functionalities and bug fixes.
*   **EditorConfig**: The project includes an `.editorconfig` file to help enforce consistent coding styles across different editors and IDEs. Ensure your editor supports EditorConfig and that it's enabled.

## Git Workflow

We utilize a branch-based Git workflow to manage changes and facilitate collaboration.

1.  **Fork the Repository (if applicable)**: If you are contributing from outside the core team, fork the main repository to your personal GitHub/GitLab account.
2.  **Clone Your Fork**: Clone your forked repository to your local machine.
    ```bash
    git clone <your-fork-url>
    cd cis-api
    ```
3.  **Create a New Branch**: For each new feature or bug fix, create a new branch from the `main` (or `develop`) branch. Use descriptive branch names (e.g., `feature/add-voting-endpoint`, `bugfix/fix-topic-validation`).
    ```bash
    git checkout main
    git pull origin main # Ensure your main is up-to-date
    git checkout -b feature/your-feature-name
    ```
4.  **Make Your Changes**: Implement your feature or bug fix. Remember to:
    *   Write clean, well-commented code.
    *   Add or update unit/integration tests as necessary.
    *   Ensure all existing tests pass.
    *   Update documentation (e.g., `README.md`, Writerside topics) if your changes affect functionality or usage.
5.  **Commit Your Changes**: Commit your changes with clear and concise commit messages. Follow the [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/) specification (e.g., `feat: Add new endpoint for ideas`, `fix: Resolve authentication issue`).
    ```bash
    git add .
    git commit -m "feat: Your descriptive commit message"
    ```
6.  **Push Your Branch**: Push your local branch to your remote fork.
    ```bash
    git push origin feature/your-feature-name
    ```
7.  **Create a Merge Request (MR) / Pull Request (PR)**:
    *   Go to the project's GitLab/GitHub repository and create a new Merge Request (MR) from your branch to the `main` (or `develop`) branch.
    *   Provide a clear description of your changes, including:
        *   What problem it solves.
        *   How it was implemented.
        *   Any relevant test results or screenshots.
        *   Link to any related issues or user stories.
8.  **Address Feedback**: Participate in the code review process. Be open to feedback and make necessary adjustments to your code.
9.  **Merge**: Once your MR/PR is approved and all CI/CD checks pass, it will be merged into the target branch.

## Reporting Bugs and Suggesting Features

If you find a bug or have a feature suggestion, please open an issue in the project's issue tracker. Provide as much detail as possible, including steps to reproduce bugs or clear descriptions of new features.

Thank you for contributing to the CIS API!
