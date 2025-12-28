package jalau.usersapi.infrastructure.mysql.mappers;

import org.springframework.stereotype.Component;
import org.springframework.context.annotation.Profile;
import jalau.usersapi.core.domain.entities.User;
import jalau.usersapi.infrastructure.mysql.entities.DbUser;

@Component("mysqlDbUserToUser")
@Profile({"mysql", "migration"})
public class DbUserToUser {
    /**
     * Converts a DbUser entity to a User domain entity.
     *
     * @param dbUser The DbUser entity to convert.
     * @return The converted User domain entity.
     */
    public User toDomain(final DbUser dbUser) {
        return new User(
            dbUser.getId(),
            dbUser.getName(),
            dbUser.getLogin(),
            dbUser.getPassword()
        );
    }
}
