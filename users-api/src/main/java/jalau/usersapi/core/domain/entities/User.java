package jalau.usersapi.core.domain.entities;

import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Getter;
import lombok.NoArgsConstructor;
import lombok.Setter;

/**
 * Represents a user entity in the system.
 * This class is mapped to the "users" table in the database.
 */
@Builder
@AllArgsConstructor @NoArgsConstructor @Setter @Getter
public class User {
    /**
     * The unique identifier for the user.
     * It is a string of 36 characters.
     */
    private String id;

    /**
     * The full name of the user.
     */
    private String name;

    /**
     * The login username for the user.
     * It must be unique across all users.
     */
    private String login;

    /**
     * The password for the user's account.
     */
    private String password;


}
