package jalau.usersapi.presentation.exceptions;

import org.springframework.dao.DataIntegrityViolationException;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.ControllerAdvice;
import org.springframework.web.bind.annotation.ExceptionHandler;

import java.util.HashMap;
import java.util.Map;

@ControllerAdvice
public class GlobalExceptionHandler {

    /**
     *  @ExceptionHandler defines which type of exception this method will handle, 
     * here it will handle data integrity errors in the database.
     *
     * @param ex DataIntegrityViolationException.class
     * @return Object
     *
     */
    @ExceptionHandler(DataIntegrityViolationException.class)
    public ResponseEntity<Object> handleDataIntegrityViolation(DataIntegrityViolationException ex) {

        String errorMessage = ex.getMostSpecificCause().getMessage();
        if (errorMessage != null && errorMessage.contains("Duplicate entry")) {

            // Create a personalized response body
            Map<String, String> errorResponse = new HashMap<>();
            errorResponse.put("error", "Conflict");
            errorResponse.put("message", "existing login.");

            // Returns a 409 Conflict status
            return new ResponseEntity<>(errorResponse, HttpStatus.CONFLICT);
        }

        //generic error
        return new ResponseEntity<>(
                "Ocorreu um erro de integridade de dados inesperado",
                HttpStatus.INTERNAL_SERVER_ERROR
        );
    }
}
