package jalau.usersapi.presentation.mappers;

import jalau.usersapi.core.domain.entities.User;
import jalau.usersapi.presentation.dtos.UserCreateDto;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

import static org.junit.jupiter.api.Assertions.*;

public class UserCreateDtoToUserMapperTest {

    private UserCreateDtoToUserMapper mapper;

    @BeforeEach
    void setUp() {
        mapper = new UserCreateDtoToUserMapper();
    }

    /**
     * Test to verify that a UserCreateDto is correctly mapped
     * to a User entity.
     */
    @Test
    void toDEntity_ShouldMapUserToUserCreateDto() {

        UserCreateDto dto = new UserCreateDto("John Doe", "jdoe", "j123");

        //act - execution and action
        User user = mapper.toEntity(dto);

        // assert - check the result
        assertNotNull(user);
        assertNull(user.getId());
        assertEquals("John Doe", user.getName());
        assertEquals("jdoe", user.getLogin());
        assertEquals("j123", user.getPassword());

    }


    /**
     * Teste para garantir que o mapeador lança uma exceção
     * quando recebe um DTO nulo.
     */
    @Test
    void toEntity_ShouldHandleNullDto() {
        // Arrange & Act & Assert
        assertThrows(NullPointerException.class, () -> mapper.toEntity(null));
    }
}