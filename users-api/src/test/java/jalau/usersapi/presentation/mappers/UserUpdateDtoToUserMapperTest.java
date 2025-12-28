package jalau.usersapi.presentation.mappers;

import jalau.usersapi.core.domain.entities.User;
import jalau.usersapi.presentation.dtos.UserUpdateDto;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.InjectMocks;
import org.mockito.junit.jupiter.MockitoExtension;


import static org.junit.jupiter.api.Assertions.*;

@ExtendWith(MockitoExtension.class)
class UserUpdateDtoToUserMapperTest {

    @InjectMocks
    private UserUpdateDtoToUserMapper mapper;

    @Test
    void shouldConvertUserUpdateDtoToUserEntity() {
        UserUpdateDto dto = new UserUpdateDto("João Silva", "joao.silva", "senha123");

        User result = mapper.toEntity(dto);

        assertNotNull(result);
        assertEquals("João Silva", result.getName());
        assertEquals("joao.silva", result.getLogin());
        assertEquals("senha123", result.getPassword());
    }

    @Test
    void shouldHandleNullDto() {
        assertThrows(NullPointerException.class, () -> mapper.toEntity(null));
    }

    @Test
    void shouldHandleDtoWithNullValues() {
        UserUpdateDto dto = new UserUpdateDto(null, null, null);

        User result = mapper.toEntity(dto);
        
        assertNotNull(result);
        assertNull(result.getName());
        assertNull(result.getLogin());
        assertNull(result.getPassword());
    }
}
