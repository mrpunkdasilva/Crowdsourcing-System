package jalau.usersapi.core.domain.services;

import jalau.usersapi.core.domain.entities.User;

import java.util.List;

public interface IUserQueryService {
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
     * @return the user with the specified ID, or an empty list if not found
     */
    List<User> getUser(String id);
}

