package jalau.usersapi.core.application;

import jalau.usersapi.core.domain.entities.User;
import jalau.usersapi.core.domain.repositories.IUserRepository;
import jalau.usersapi.core.domain.services.IAuthQueryService;
import org.springframework.context.annotation.Profile;
import org.springframework.stereotype.Service;

import java.util.Base64;

@Service
@Profile("!migration")
public class AuthQueryService implements IAuthQueryService {

    private final IUserRepository userRepository;

    public AuthQueryService(IUserRepository userRepository) {
        this.userRepository = userRepository;
    }

    @Override
    public User authorize(String authorizationHeader) {
        if (authorizationHeader == null || !authorizationHeader.startsWith("Basic ")) {
            return null;
        }

        String base64Credentials = authorizationHeader.substring("Basic ".length()).trim();
        byte[] credDecoded = Base64.getDecoder().decode(base64Credentials);
        String credentials = new String(credDecoded);
        // credentials = username:password
        final String[] values = credentials.split(":", 2);
        if (values.length != 2) {
            return null;
        }

        String login = values[0];
        String password = values[1];

        User user = userRepository.getUserByLogin(login);
        if (user != null && user.getPassword().equals(password)) {
            return user;
        }
        return null;
    }
}
