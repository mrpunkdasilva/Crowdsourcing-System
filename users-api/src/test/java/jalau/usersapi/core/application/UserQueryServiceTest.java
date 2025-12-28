package jalau.usersapi.core.application;

import jalau.usersapi.core.domain.entities.User;
import jalau.usersapi.core.domain.repositories.IUserRepository;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.junit.jupiter.MockitoExtension;

import java.util.Arrays;
import java.util.List;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

@ExtendWith(MockitoExtension.class)
class UserQueryServiceTest {

    @Mock
    private IUserRepository userRepository;

    @InjectMocks
    private UserQueryService userQueryService;

    @Test
    void shouldReturnListOfUsers() {
        List<User> expectedUsers = Arrays.asList(
            new User("1", "User1", "user1", "pass1"),
            new User("2", "User2", "user2", "pass2")
        );
        when(userRepository.getUsers()).thenReturn(expectedUsers);
        
        List<User> result = userQueryService.getUsers();
        
        assertEquals(2, result.size());
        assertEquals("User1", result.get(0).getName());
        verify(userRepository).getUsers();
    }

    @Test
    void shouldReturnEmptyListWhenNoUsers() {
        when(userRepository.getUsers()).thenReturn(List.of());
        
        List<User> result = userQueryService.getUsers();
        
        assertTrue(result.isEmpty());
        verify(userRepository).getUsers();
    }
}
