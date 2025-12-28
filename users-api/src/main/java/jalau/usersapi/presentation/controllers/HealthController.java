package jalau.usersapi.presentation.controllers;

import org.springframework.context.annotation.Profile;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

/**
 * A simple controller to check the health of the application.
 */
@RestController
@RequestMapping("/api/v1/health")
@Profile("!migration")
public class HealthController {

    /**
     * Returns a 200 OK status to indicate that the application is running.
     *
     * @return A ResponseEntity with a 200 OK status.
     */
    @GetMapping
    public ResponseEntity<Void> healthCheck() {
        return ResponseEntity.ok().build();
    }
}
