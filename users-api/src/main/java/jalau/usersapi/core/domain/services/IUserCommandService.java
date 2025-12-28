package jalau.usersapi.core.domain.services;

import jalau.usersapi.core.domain.entities.User;

public interface IUserCommandService {
    /**
     * Creates a new user.
     * The input User instance must have a null ID, and the output User instance must have a non-null ID.
     *
     * @param user the user to create
     * @return the created user
     */
    User createUser(User user);

    /**
     * Updates an existing user.
     * The input User instance must have a non-null ID, and the output User instance must have a non-null ID.
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
}
