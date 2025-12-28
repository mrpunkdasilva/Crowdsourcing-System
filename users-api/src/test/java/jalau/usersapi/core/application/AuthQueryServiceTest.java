package jalau.usersapi.core.application;

import jalau.usersapi.core.domain.entities.User;
import jalau.usersapi.core.domain.repositories.IUserRepository;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.MockitoAnnotations;

import java.util.Base64;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.when;

class AuthQueryServiceTest {

    @Mock
    private IUserRepository userRepository;

    @InjectMocks
    private AuthQueryService authQueryService;

    @BeforeEach
    void setUp() {
        MockitoAnnotations.openMocks(this);
    }

    @Test
    void authorize_ValidBasicAuthHeader_ReturnsUser() {
        String login = "testuser";
        String password = "testpassword";
        String credentials = login + ":" + password;
        String base64Credentials = Base64.getEncoder().encodeToString(credentials.getBytes());
        String authorizationHeader = "Basic " + base64Credentials;

        User expectedUser = User.builder()
                .login(login)
                .password(password)
                .build();

        when(userRepository.getUserByLogin(login)).thenReturn(expectedUser);

        User result = authQueryService.authorize(authorizationHeader);

        assertNotNull(result);
        assertEquals(expectedUser.getLogin(), result.getLogin());
        assertEquals(expectedUser.getPassword(), result.getPassword());
    }

    @Test
    void authorize_InvalidBasicAuthHeader_ReturnsNull() {
        String authorizationHeader = "InvalidHeader";

        User result = authQueryService.authorize(authorizationHeader);

        assertNull(result);
    }

    @Test
    void authorize_UserNotFound_ReturnsNull() {
        String login = "nonexistent";
        String password = "anypassword";
        String credentials = login + ":" + password;
        String base64Credentials = Base64.getEncoder().encodeToString(credentials.getBytes());
        String authorizationHeader = "Basic " + base64Credentials;

        when(userRepository.getUserByLogin(login)).thenReturn(null);

        User result = authQueryService.authorize(authorizationHeader);

        assertNull(result);
    }

    @Test
    void authorize_IncorrectPassword_ReturnsNull() {
        String login = "testuser";
        String correctPassword = "correctpassword";
        String incorrectPassword = "wrongpassword";
        String credentials = login + ":" + incorrectPassword;
        String base64Credentials = Base64.getEncoder().encodeToString(credentials.getBytes());
        String authorizationHeader = "Basic " + base64Credentials;

        User storedUser = User.builder()
                .login(login)
                .password(correctPassword)
                .build();

        when(userRepository.getUserByLogin(login)).thenReturn(storedUser);

        User result = authQueryService.authorize(authorizationHeader);

        assertNull(result);
    }

    @Test
    void authorize_MalformedCredentials_ReturnsNull() {
        String malformedCredentials = "malformed"; // Missing colon
        String base64Credentials = Base64.getEncoder().encodeToString(malformedCredentials.getBytes());
        String authorizationHeader = "Basic " + base64Credentials;

        User result = authQueryService.authorize(authorizationHeader);

        assertNull(result);
    }

    @Test
    void authorize_NullAuthorizationHeader_ReturnsNull() {
        User result = authQueryService.authorize(null);
        assertNull(result);
    }

    @Test
    void authorize_EmptyAuthorizationHeader_ReturnsNull() {
        User result = authQueryService.authorize("");
        assertNull(result);
    }

    @Test
    void authorize_AuthorizationHeaderWithoutBasicPrefix_ReturnsNull() {
        String login = "testuser";
        String password = "testpassword";
        String credentials = login + ":" + password;
        String base64Credentials = Base64.getEncoder().encodeToString(credentials.getBytes());
        String authorizationHeader = base64Credentials; // Missing "Basic " prefix

        User result = authQueryService.authorize(authorizationHeader);

        assertNull(result);
    }
}
