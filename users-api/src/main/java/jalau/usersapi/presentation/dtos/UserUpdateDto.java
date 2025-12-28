package jalau.usersapi.presentation.dtos;

/**
 * Represents the data transfer object for updating an existing user.
 * This record encapsulates the information that can be updated for a user.
 *
 * @param name     The new name of the user.
 * @param login    The new login identifier for the user.
 * @param password The new password for the user.
 */
public record UserUpdateDto(String name, String login, String password) {
}