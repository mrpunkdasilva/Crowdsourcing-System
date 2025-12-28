package jalau.usersapi.infrastructure.mysql;

import jalau.usersapi.infrastructure.mysql.entities.DbUser;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.Optional; // Import Optional

@Repository
public interface SpringDataUserRepository extends JpaRepository<DbUser, String> {
    Optional<DbUser> findByLogin(String login);
}
