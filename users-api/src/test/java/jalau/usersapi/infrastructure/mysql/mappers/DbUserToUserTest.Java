package jalau.usersapi.infrastructure.mysql.mappers;

import jalau.usersapi.core.domain.entities.User;
import jalau.usersapi.infrastructure.mysql.entities.DbUser;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;

class DbUserToUserTest {

    @Test
    void shouldConvertDbUserToUser() {
        DbUserToUser mapper = new DbUserToUser();
        DbUser dbUser = new DbUser("123", "John Doe", "johndoe", "password");
        
        User result = mapper.toDomain(dbUser);
        
        assertEquals("123", result.getId());
        assertEquals("John Doe", result.getName());
        assertEquals("johndoe", result.getLogin());
        assertEquals("password", result.getPassword());
    }
}
