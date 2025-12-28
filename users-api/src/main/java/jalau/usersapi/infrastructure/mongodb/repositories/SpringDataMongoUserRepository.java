package jalau.usersapi.infrastructure.mongodb.repositories;

import jalau.usersapi.infrastructure.mongodb.entities.DbUser;
import org.springframework.data.mongodb.repository.MongoRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface SpringDataMongoUserRepository extends MongoRepository<DbUser, String> {
    boolean existsByLogin(String login);
    java.util.Optional<DbUser> findByLogin(String login);
}
