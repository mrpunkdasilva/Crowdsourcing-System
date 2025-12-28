package jalau.usersapi.infrastructure.mongodb.entities;

import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;
import org.springframework.data.annotation.Id;
import org.springframework.data.mongodb.core.mapping.Document;

@Data
@AllArgsConstructor
@NoArgsConstructor
@Document(collection = "users")
public class DbUser {
    @Id
    private String id;
    private String sql_id; // Original MySQL ID
    private String name;
    private String login;
    private String password;
}
