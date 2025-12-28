package jalau.usersapi.presentation.validators;

import jalau.usersapi.presentation.dtos.UserUpdateDto;
import org.springframework.stereotype.Component;
import org.springframework.validation.Errors;
import org.springframework.validation.Validator;

@Component
public class UserUpdateDtoValidator implements Validator {

    @Override
    public boolean supports(Class<?> clazz) {
        return UserUpdateDto.class.isAssignableFrom(clazz);
    }

    /**
     * Validate a Optional camps of UserUpdateDto
     *
     * @param target object that is validate.
     * @param errors 
     */
    @Override
    public void validate(Object target, Errors errors) {
        UserUpdateDto dto = (UserUpdateDto) target;

         // Validation for name - only valid if not null AND empty
        if (dto.name() != null && dto.name().trim().isEmpty()) {
            errors.rejectValue("name", "name.empty", "Name cannot be empty");
        }

        // Validation for login - only valid if not null AND empty
        if (dto.login() != null && dto.login().trim().isEmpty()) {
            errors.rejectValue("login", "login.empty", "Login cannot be empty");
        }

        // Validation for password - only valid if not null AND empty
        if (dto.password() != null && dto.password().trim().isEmpty()) {
            errors.rejectValue("password", "password.empty", "Password cannot be empty");
        }
    }
}