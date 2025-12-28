package jalau.usersapi.core.application;

import jalau.usersapi.core.domain.repositories.IUserRepository;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.MockitoAnnotations;

import static org.mockito.Mockito.verify;

public class UserCommandServiceTest {

    @Mock
    private IUserRepository userRepository;

    @InjectMocks
    private UserCommandService userCommandService;

    @BeforeEach
    void setUp() {
        MockitoAnnotations.openMocks(this);
    }

    @Test
    void shouldCallRepositoryDeleteUserWhenDeletingUser() {
        // Arrange
        String userId = "test-uuid-123";

        // Act
        userCommandService.deleteUser(userId);

        // Assert
        // Verify that the repository's deleteUser method was called with the correct ID
        verify(userRepository).deleteUser(userId);
    }
}

