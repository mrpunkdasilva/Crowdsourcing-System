package jalau.usersapi.infrastructure.mongodb.mappers;

import jalau.usersapi.core.domain.entities.User;
import jalau.usersapi.infrastructure.mongodb.entities.DbUser;
import org.springframework.context.annotation.Profile;
import org.springframework.stereotype.Component;

@Component("mongoDbUserToUser")
@Profile({"mongodb", "migration"})
public class DbUserToUser {
    public User toDomain(DbUser dbUser) {
        return new User(dbUser.getId(), dbUser.getName(), dbUser.getLogin(), dbUser.getPassword());
    }
}
