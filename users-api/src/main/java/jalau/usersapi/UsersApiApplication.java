package jalau.usersapi;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;

@SpringBootApplication
public final class UsersApiApplication {

    /**
     * Private constructor to prevent instantiation of this utility class.
     */
    private UsersApiApplication() {
    }

    /**
     * The main entry point of the Spring Boot application.
     *
     * @param args The command line arguments.
     */
    public static void main(final String[] args) {
        SpringApplication.run(UsersApiApplication.class, args);
    }
}
