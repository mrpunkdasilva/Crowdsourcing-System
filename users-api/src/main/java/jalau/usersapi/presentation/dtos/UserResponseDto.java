package jalau.usersapi.presentation.dtos;

/**
 * Represents the data transfer object for a user response.
 * This record encapsulates the information about a user that is sent back to the client.
 *
 * @param id    The unique identifier of the user.
 * @param name  The name of the user.
 * @param login The login identifier of the user.
 */
public record UserResponseDto(
        String id,
        String name,
        String login
) {
}