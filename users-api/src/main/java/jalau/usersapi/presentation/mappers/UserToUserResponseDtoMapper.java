package jalau.usersapi.presentation.mappers;

import jalau.usersapi.core.domain.entities.User;
import jalau.usersapi.presentation.dtos.UserResponseDto;
import org.springframework.stereotype.Component;

/**
 * Mapper to convert Domain User to Presentation UserResponseDto.
 */
@Component
public class UserToUserResponseDtoMapper {
    /**
     * Converts a User domain entity to a UserResponseDto.
     *
     * @param user The User domain entity to convert.
     * @return The converted UserResponseDto.
     */
    public UserResponseDto toDto(final User user) {
        return new UserResponseDto(
            user.getId(),
            user.getName(),
            user.getLogin()
        );
    }
}