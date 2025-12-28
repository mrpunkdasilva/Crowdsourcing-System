package jalau.usersapi.presentation.dtos;

import jakarta.validation.constraints.NotBlank;

/**
 * Represents the data transfer object for creating a new user.
 * This record encapsulates the information required to create a new user entity.
 * NotBlank verifica se a string não é nula
 *
 * @param name     The name of the user.
 * @param login    The login identifier for the new user.
 * @param password The password for the new user.
 *
 */
public record UserCreateDto(

    @NotBlank(message = "Mandatory")
     String name,

    @NotBlank(message = "Mandatory")
     String login,

    @NotBlank(message = "Mandatory")
     String password

) { }
