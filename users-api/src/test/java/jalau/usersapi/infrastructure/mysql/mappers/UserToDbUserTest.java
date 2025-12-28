package jalau.usersapi.infrastructure.mysql.mappers;

import jalau.usersapi.core.domain.entities.User;
import jalau.usersapi.infrastructure.mysql.entities.DbUser;
import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;

class UserToDbUserTest {

    @Test
    void shouldConvertUserToDbUser() {
        UserToDbUser mapper = new UserToDbUser();
        User user = new User("456", "Jane Smith", "janesmith", "mypassword");
        
        DbUser result = mapper.toStructure(user);
        
        assertEquals("456", result.getId());
        assertEquals("Jane Smith", result.getName());
        assertEquals("janesmith", result.getLogin());
        assertEquals("mypassword", result.getPassword());
    }
}
