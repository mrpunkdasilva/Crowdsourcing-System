package jalau.usersapi.presentation.controllers;

import jalau.usersapi.core.domain.entities.User;
import jalau.usersapi.core.domain.services.IUserQueryService;
import jalau.usersapi.presentation.dtos.UserResponseDto;
import jalau.usersapi.presentation.mappers.UserToUserResponseDtoMapper;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.junit.jupiter.MockitoExtension;
import org.springframework.http.ResponseEntity;

import java.util.Arrays;
import java.util.List;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

@ExtendWith(MockitoExtension.class)
class UserQueryControllerTest {

    @Mock
    private IUserQueryService userQueryService;

    @Mock
    private UserToUserResponseDtoMapper mapper;

    @InjectMocks
    private UserQueryController userQueryController;

    private User user1;
    private User user2;
    private UserResponseDto dto1;
    private UserResponseDto dto2;

    @BeforeEach
    void setUp() {
        user1 = new User("1", "João Silva", "joaosilva", "senha123");
        user2 = new User("2", "Maria Santos", "mariasantos", "senha456");
        
        dto1 = new UserResponseDto("1", "João Silva", "joaosilva");
        dto2 = new UserResponseDto("2", "Maria Santos", "mariasantos");
    }

    @Test
    void getUsers_shouldReturnListOfUsers() {
        List<User> users = Arrays.asList(user1, user2);
        when(userQueryService.getUsers()).thenReturn(users);
        when(mapper.toDto(user1)).thenReturn(dto1);
        when(mapper.toDto(user2)).thenReturn(dto2);

        ResponseEntity<List<UserResponseDto>> response = userQueryController.getUsers();

        assertNotNull(response);
        assertEquals(200, response.getStatusCodeValue());
        assertEquals(2, response.getBody().size());
        assertEquals(dto1, response.getBody().get(0));
        assertEquals(dto2, response.getBody().get(1));
        
        verify(userQueryService, times(1)).getUsers();
        verify(mapper, times(1)).toDto(user1);
        verify(mapper, times(1)).toDto(user2);
    }

    @Test
    void getUsers_whenNoUsers_shouldReturnEmptyList() {
        when(userQueryService.getUsers()).thenReturn(List.of());

        ResponseEntity<List<UserResponseDto>> response = userQueryController.getUsers();

        assertNotNull(response);
        assertEquals(200, response.getStatusCodeValue());
        assertTrue(response.getBody().isEmpty());
        
        verify(userQueryService, times(1)).getUsers();
        verify(mapper, never()).toDto(any());
    }

    @Test
    void getUser_shouldReturnUserById() {
        String userId = "1";
        List<User> userList = List.of(user1);
        when(userQueryService.getUser(userId)).thenReturn(userList);
        when(mapper.toDto(user1)).thenReturn(dto1);

        ResponseEntity<List<UserResponseDto>> response = userQueryController.getUser(userId);

        assertNotNull(response);
        assertEquals(200, response.getStatusCodeValue());
        assertEquals(1, response.getBody().size());
        assertEquals(dto1, response.getBody().get(0));
        
        verify(userQueryService, times(1)).getUser(userId);
        verify(mapper, times(1)).toDto(user1);
    }

    @Test
    void getUser_whenUserNotFound_shouldReturnEmptyList() {
        String userId = "999";
        when(userQueryService.getUser(userId)).thenReturn(List.of());

        ResponseEntity<List<UserResponseDto>> response = userQueryController.getUser(userId);

        assertNotNull(response);
        assertEquals(200, response.getStatusCodeValue());
        assertTrue(response.getBody().isEmpty());
        
        verify(userQueryService, times(1)).getUser(userId);
        verify(mapper, never()).toDto(any());
    }

    @Test
    void getUser_withMultipleUsersSameId_shouldReturnAll() {
        String userId = "1";
        User user1Duplicate = new User("1", "João Silva Duplicate", "joaosilva2", "senha789");
        List<User> users = Arrays.asList(user1, user1Duplicate);
        
        UserResponseDto dto1Duplicate = new UserResponseDto("1", "João Silva Duplicate", "joaosilva2");
        
        when(userQueryService.getUser(userId)).thenReturn(users);
        when(mapper.toDto(user1)).thenReturn(dto1);
        when(mapper.toDto(user1Duplicate)).thenReturn(dto1Duplicate);

        ResponseEntity<List<UserResponseDto>> response = userQueryController.getUser(userId);

        assertNotNull(response);
        assertEquals(200, response.getStatusCodeValue());
        assertEquals(2, response.getBody().size());
        assertEquals(dto1, response.getBody().get(0));
        assertEquals(dto1Duplicate, response.getBody().get(1));
        
        verify(userQueryService, times(1)).getUser(userId);
        verify(mapper, times(2)).toDto(any(User.class));
    }
}
