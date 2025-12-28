# Checkstyle Configuration

This directory contains the Checkstyle configuration file (`checkstyle.xml`) used in this project to enforce a consistent coding style and best practices.

## Purpose

Checkstyle is a development tool to help programmers write Java code that adheres to a coding standard. It automates the process of checking Java code for adherence to a set of coding rules.

## Applied Style Guide

This configuration is based on a custom style guide tailored for this project. It aims to improve code readability, maintainability, and consistency across the codebase.

## Key Rules and Examples

Here are some of the key rules enforced by this Checkstyle configuration:

### 1. Indentation

-   **Rule**: Uses 4 spaces for indentation.
-   **Example (Correct)**:
    ```java
    public class MyClass {
        public void myMethod() {
            // Code indented with 4 spaces
            if (true) {
                System.out.println("Hello");
            }
        }
    }
    ```

### 2. Brace Style (Left Curly)

-   **Rule**: Opening curly braces (`{`) for classes, methods, and control structures (if, for, while, etc.) must be on the same line as the declaration.
-   **Example (Correct)**:
    ```java
    public class MyClass {
        public void myMethod() {
            if (condition) {
                // ...
            }
        }
    }
    ```
-   **Example (Incorrect)**:
    ```java
    public class MyClass
    { // Incorrect
        public void myMethod()
        { // Incorrect
            // ...
        }
    }
    ```

### 3. Missing Javadoc

-   **Rule**: All public classes, interfaces, methods, and constructors must have Javadoc comments.
-   **Example (Correct)**:
    ```java
    /**
     * This is a Javadoc comment for the class.
     */
    public class MyClass {
        /**
         * This is a Javadoc comment for the method.
         * @param name The name of the user.
         */
        public void greet(String name) {
            // ...
        }
    }
    ```

### 4. Final Parameters

-   **Rule**: Method parameters should be declared as `final` to indicate that their values will not be changed within the method.
-   **Example (Correct)**:
    ```java
    public void process(final String data) {
        // data cannot be reassigned
    }
    ```
-   **Example (Incorrect)**:
    ```java
    public void process(String data) { // Incorrect
        // data can be reassigned
    }
    ```

### 5. Avoid Star Imports

-   **Rule**: Wildcard imports (`import com.example.package.*`) are generally discouraged. Explicitly import each class.
-   **Example (Correct)**:
    ```java
    import java.util.List;
    import java.util.ArrayList;
    ```
-   **Example (Incorrect)**:
    ```java
    import java.util.*; // Incorrect
    ```

### 6. TODO Comments

-   **Rule**: `TODO` comments should follow a specific format (e.g., `// TODO: [Description of task]`).
-   **Example (Correct)**:
    ```java
    // TODO: Implement error handling for this section
    ```

## How to Use

The Checkstyle plugin is integrated into the Maven build process. It runs automatically during the `validate` phase (e.g., when running `mvn clean install` or `mvn verify`).

To run Checkstyle checks manually:

```bash
mvn checkstyle:check
```

To generate a Checkstyle report:

```bash
mvn checkstyle:checkstyle
```

The report will be generated in `target/site/checkstyle.html`.

## Contributing

When contributing code, please ensure your changes adhere to these Checkstyle rules. The CI/CD pipeline will automatically check for compliance.
