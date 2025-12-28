package jalau.usersapi.core.domain.repositories;

import jalau.usersapi.core.domain.entities.User;

import java.util.List;

public interface IUserRepository {
    /**
     * Creates a new user.
     *
     * @param user the user to create
     * @return the created user
     */
    User createUser(User user);

    /**
     * Updates an existing user.
     *
     * @param user the user to update
     * @return the updated user
     */
    User updateUser(User user);

    /**
     * Deletes a user by their ID.
     *
     * @param id the ID of the user to delete
     */
    void deleteUser(String id);

    /**
     * Retrieves all users.
     *
     * @return a list of all users
     */
    List<User> getUsers();

    /**
     * Retrieves a user by their ID.
     *
     * @param id the ID of the user to retrieve
     * @return the user with the specified ID, or null if not found
     */
    User getUser(String id);

    /**
     * Retrieves a user by their login.
     *
     * @param login the login of the user to retrieve
     * @return the user with the specified login, or null if not found
     */
    User getUserByLogin(String login);
}
