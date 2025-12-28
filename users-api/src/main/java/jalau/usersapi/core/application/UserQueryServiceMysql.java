package jalau.usersapi.core.application;

import jalau.usersapi.core.domain.entities.User;
import jalau.usersapi.core.domain.repositories.IUserRepository;
import jalau.usersapi.core.domain.services.IUserQueryService;
import org.springframework.beans.factory.annotation.Qualifier;
import org.springframework.context.annotation.Profile;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
@Profile("mysql")
public class UserQueryServiceMysql implements IUserQueryService {

    private final IUserRepository userRepository;

    public UserQueryServiceMysql(@Qualifier("mysqlUserRepository") final IUserRepository userRepository) {
        this.userRepository = userRepository;
    }

    @Override
    public List<User> getUsers() {
        return userRepository.getUsers();
    }

    @Override
    public List<User> getUser(final String id) {
        User user = userRepository.getUser(id);
        return user != null ? List.of(user) : List.of();
    }
}