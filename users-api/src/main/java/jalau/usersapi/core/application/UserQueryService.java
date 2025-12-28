package jalau.usersapi.core.application;

import jalau.usersapi.core.domain.entities.User;
import jalau.usersapi.core.domain.repositories.IUserRepository;
import jalau.usersapi.core.domain.services.IUserQueryService;
import org.springframework.beans.factory.annotation.Qualifier;
import org.springframework.context.annotation.Profile;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
@Profile("mongodb")
class UserQueryService implements IUserQueryService {

    private final IUserRepository userRepository;

    public UserQueryService(@Qualifier("mongoUserRepository") final IUserRepository userRepository) {
        this.userRepository = userRepository;
    }

    @Override
    public List<User> getUsers() {
        return userRepository.getUsers();
    }

    // Point to be discussed**
    // Method: getUser by Id (Hope one result)
    // Repository return only a User
    // Service return a List<User>
    @Override
    public List<User> getUser(final String id) {
        User user = userRepository.getUser(id);
        return user != null ? List.of(user) : List.of();
    }
    
    // all methods throw NotImplemented exception
}
