package jalau.usersapi.infrastructure.mysql.mappers;

import org.springframework.stereotype.Component;
import org.springframework.context.annotation.Profile;
import jalau.usersapi.core.domain.entities.User;
import jalau.usersapi.infrastructure.mysql.entities.DbUser;

@Component("mysqlUserToDbUser")
@Profile({"mysql", "migration"})
public class UserToDbUser {
    /**
     * Converts a User domain entity to a DbUser entity.
     *
     * @param user The User domain entity to convert.
     * @return The converted DbUser entity.
     */
    public DbUser toStructure(final User user) {
        return new DbUser(
            user.getId(),
            user.getName(),
            user.getLogin(),
            user.getPassword()
        );
    }
}