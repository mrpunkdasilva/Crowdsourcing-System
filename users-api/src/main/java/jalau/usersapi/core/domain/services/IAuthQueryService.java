package jalau.usersapi.core.domain.services;

import jalau.usersapi.core.domain.entities.User;

/**
 * Interface for the authentication query service.
 */
public interface IAuthQueryService {
    /**
     * Authorizes a user based on the provided authorization header.
     *
     * @param authorizationHeader the authorization header
     * @return the authorized user, or null if not authorized
     */
    User authorize(String authorizationHeader);
}
