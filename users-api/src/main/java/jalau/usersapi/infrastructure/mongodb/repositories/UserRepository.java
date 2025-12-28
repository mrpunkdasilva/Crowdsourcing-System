package jalau.usersapi.infrastructure.mongodb.repositories;

import jalau.usersapi.core.domain.entities.User;
import jalau.usersapi.core.domain.repositories.IUserRepository;
import org.springframework.context.annotation.Profile;
import org.springframework.stereotype.Repository;

import jalau.usersapi.infrastructure.mongodb.mappers.DbUserToUser;
import jalau.usersapi.infrastructure.mongodb.mappers.UserToDbUser;
import org.springframework.beans.factory.annotation.Qualifier;
import jalau.usersapi.infrastructure.mongodb.mappers.UserToDbUser;
import org.springframework.security.core.userdetails.UsernameNotFoundException;
import jalau.usersapi.infrastructure.mongodb.entities.DbUser;


import java.util.List;
import java.util.Optional;

@Repository("mongoUserRepository")
@Profile({"mongodb", "migration"})
public class UserRepository implements IUserRepository {

    private final SpringDataMongoUserRepository springDataMongoUserRepository;
    private final DbUserToUser dbUserToUser;
    private final UserToDbUser userToDbUser;

    public UserRepository(SpringDataMongoUserRepository springDataMongoUserRepository,
                          @Qualifier("mongoDbUserToUser") DbUserToUser dbUserToUser,
                          @Qualifier("mongoUserToDbUser") UserToDbUser userToDbUser) {
        this.springDataMongoUserRepository = springDataMongoUserRepository;
        this.dbUserToUser = dbUserToUser;
        this.userToDbUser = userToDbUser;
    }


    @Override
    public User createUser(User user) {
        DbUser dbUser = userToDbUser.toStructure(user);
        DbUser savedDbUser = springDataMongoUserRepository.save(dbUser);
        return dbUserToUser.toDomain(savedDbUser);
    }

    @Override
    public User updateUser(User user) {
        Optional<DbUser> existing = springDataMongoUserRepository.findById(user.getId());
        if (existing.isEmpty()) {
            return null;
        }

        User currentUser = dbUserToUser.toDomain(existing.get());
        User updatedUser = applyUpdates(user, currentUser);
        DbUser dbUserToUpdate = userToDbUser.toStructure(updatedUser);

        DbUser saved = springDataMongoUserRepository.save(dbUserToUpdate);
        return dbUserToUser.toDomain(saved);
    }

    private User applyUpdates(User newUser, User user) {
        if (newUser.getName() != null) user.setName(newUser.getName());
        if (newUser.getLogin() != null) user.setLogin(newUser.getLogin());
        if (newUser.getPassword() != null) user.setPassword(newUser.getPassword());
        return user;
    }

    @Override
    public void deleteUser(String id) {
        if (!springDataMongoUserRepository.existsById(id)) {
            throw new UsernameNotFoundException(id + " not found");
        }
        springDataMongoUserRepository.deleteById(id);
    }

    @Override
    public List<User> getUsers() {
        List<DbUser> dbUsers = springDataMongoUserRepository.findAll();
        return dbUsers.stream()
                .map(dbUserToUser::toDomain)
                .toList();    }

    @Override
    public User getUser(String id) {
        Optional<DbUser> dbUser = springDataMongoUserRepository.findById(id);
        return dbUser.map(dbUserToUser::toDomain).orElse(null);    }

    @Override
    public User getUserByLogin(String login) {
        return springDataMongoUserRepository.findByLogin(login)
                .map(dbUserToUser::toDomain)
                .orElse(null);
    }

}
