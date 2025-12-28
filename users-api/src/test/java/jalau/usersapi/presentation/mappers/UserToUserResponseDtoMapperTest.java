package jalau.usersapi.presentation.mappers;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertNotNull;
import static org.junit.jupiter.api.Assertions.assertThrows;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

import jalau.usersapi.core.domain.entities.User;
import jalau.usersapi.presentation.dtos.UserResponseDto;

public class UserToUserResponseDtoMapperTest {
    private UserToUserResponseDtoMapper mapper;

    @BeforeEach
    void setUp() {
        mapper = new UserToUserResponseDtoMapper();
    }

    @Test
    void toDto_ShouldMapUserToUserResponseDto() {
        
        User user = User.builder()
                .id("1234-5678")
                .name("John Doe")
                .login("jdoe")
                .password("secret") // Not exist in DTO
                .build();

        UserResponseDto dto = mapper.toDto(user);

        assertNotNull(dto);
        assertEquals("1234-5678", dto.id());
        assertEquals("John Doe", dto.name());
        assertEquals("jdoe", dto.login());
    }

    @Test
    void toDto_ShouldHandleNullUser() {
        assertThrows(NullPointerException.class, () -> mapper.toDto(null));
    }
}
