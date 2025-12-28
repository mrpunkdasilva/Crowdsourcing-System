package jalau.usersapi.presentation.mappers;

import jalau.usersapi.core.domain.entities.User;
import jalau.usersapi.presentation.dtos.UserUpdateDto;
import org.springframework.stereotype.Component;

@Component
public class UserUpdateDtoToUserMapper {

    /**
     * Converts a UserUpdateDto to a User entity.
     *
     * @param dto The UserUpdateDto to convert.
     * @return The converted User entity.
     */
    public User toEntity(final UserUpdateDto dto) {
        return User.builder()
                .name(dto.name())
                .login(dto.login())
                .password(dto.password())
                .build();
    }

}
