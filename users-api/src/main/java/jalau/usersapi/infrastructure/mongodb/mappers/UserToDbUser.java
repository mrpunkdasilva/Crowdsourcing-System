package jalau.usersapi.infrastructure.mongodb.mappers;

import jalau.usersapi.core.domain.entities.User;
import jalau.usersapi.infrastructure.mongodb.entities.DbUser;
import org.springframework.context.annotation.Profile;
import org.springframework.stereotype.Component;

@Component("mongoUserToDbUser")
@Profile({"mongodb", "migration"})
public class UserToDbUser {
    public DbUser toStructure(User user) {
        DbUser dbUser = new DbUser();
        dbUser.setSql_id(user.getId()); // Set the original user ID as sql_id
        dbUser.setName(user.getName());
        dbUser.setLogin(user.getLogin());
        dbUser.setPassword(user.getPassword());
        return dbUser;
    }
}
