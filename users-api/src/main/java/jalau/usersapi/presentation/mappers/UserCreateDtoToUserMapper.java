package jalau.usersapi.presentation.mappers;

import jalau.usersapi.core.domain.entities.User;
import jalau.usersapi.presentation.dtos.UserCreateDto;
import org.springframework.stereotype.Component;

@Component
public class UserCreateDtoToUserMapper {
    /**
     * Converts a UserCreateDto to a User entity.
     *
     * @param dto The UserCreateDto to convert.
     * @return The converted User entity.
     *
     */
    public User toEntity(UserCreateDto dto) {
        return User.builder()
                .name(dto.name())
                .login(dto.login())
                .password(dto.password())
                .build();
    }
}
