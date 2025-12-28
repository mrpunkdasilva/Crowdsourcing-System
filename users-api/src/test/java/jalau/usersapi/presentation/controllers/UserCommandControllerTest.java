package jalau.usersapi.presentation.controllers;

import jalau.usersapi.core.domain.entities.User;
import jalau.usersapi.core.domain.services.IUserCommandService;

import jalau.usersapi.presentation.controllers.UserCommandController;
import jalau.usersapi.presentation.dtos.UserCreateDto;
import jalau.usersapi.presentation.dtos.UserResponseDto;
import jalau.usersapi.presentation.dtos.UserUpdateDto;
import jalau.usersapi.presentation.mappers.UserCreateDtoToUserMapper;
import jalau.usersapi.presentation.mappers.UserToUserResponseDtoMapper;
import jalau.usersapi.presentation.mappers.UserUpdateDtoToUserMapper;
import jalau.usersapi.presentation.validators.UserUpdateDtoValidator;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.junit.jupiter.MockitoExtension;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.validation.BindingResult;
import org.springframework.validation.FieldError;

import java.util.HashMap;
import java.util.Map;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.ArgumentMatchers.any;
import static org.mockito.Mockito.*;

@ExtendWith(MockitoExtension.class)
class UserCommandControllerTest {

    @Mock
    private IUserCommandService userCommandService;

    @Mock
    private UserCreateDtoToUserMapper createMapper;

    @Mock
    private UserUpdateDtoToUserMapper updateMapper;

    @Mock
    private UserToUserResponseDtoMapper responseMapper;

    @Mock
    private UserUpdateDtoValidator validator;

    @Mock
    private BindingResult bindingResult;

    @InjectMocks
    private UserCommandController controller;

    @Test
    void shouldCreateUserSuccessfully() {
        UserCreateDto createDto = new UserCreateDto("Maria", "maria.login", "senha123");
        User userEntity = new User("1", "Maria", "maria.login", "senha123");
        User createdUser = new User("1", "Maria", "maria.login", "senha123");
        UserResponseDto responseDto = new UserResponseDto("1", "Maria", "maria.login");

        when(createMapper.toEntity(createDto)).thenReturn(userEntity);
        when(userCommandService.createUser(userEntity)).thenReturn(createdUser);
        when(responseMapper.toDto(createdUser)).thenReturn(responseDto);

        ResponseEntity<UserResponseDto> response = controller.createUser(createDto);

        assertEquals(HttpStatus.CREATED, response.getStatusCode());
        assertNotNull(response.getBody());
        assertEquals("1", response.getBody().id());
        assertEquals("Maria", response.getBody().name());
        assertEquals("maria.login", response.getBody().login());

        verify(createMapper).toEntity(createDto);
        verify(userCommandService).createUser(userEntity);
        verify(responseMapper).toDto(createdUser);
    }

    @Test
    void shouldUpdateUserSuccessfully() {
        String userId = "123";
        UserUpdateDto updateDto = new UserUpdateDto("Maria Updated", "maria.updated", "novaSenha");
        User userEntity = new User(userId, "Maria Updated", "maria.updated", "novaSenha");
        User updatedUser = new User(userId, "Maria Updated", "maria.updated", "novaSenha");
        UserResponseDto responseDto = new UserResponseDto(userId, "Maria Updated", "maria.updated");

        when(bindingResult.hasErrors()).thenReturn(false);
        when(updateMapper.toEntity(updateDto)).thenReturn(userEntity);
        when(userCommandService.updateUser(userEntity)).thenReturn(updatedUser);
        when(responseMapper.toDto(updatedUser)).thenReturn(responseDto);

        ResponseEntity<?> response = controller.updateUser(userId, updateDto, bindingResult);

        assertEquals(HttpStatus.OK, response.getStatusCode());
        assertTrue(response.getBody() instanceof UserResponseDto);
        
        UserResponseDto responseBody = (UserResponseDto) response.getBody();
        assertEquals(userId, responseBody.id());
        assertEquals("Maria Updated", responseBody.name());
        assertEquals("maria.updated", responseBody.login());

        verify(validator).validate(updateDto, bindingResult);
        verify(updateMapper).toEntity(updateDto);
        verify(userCommandService).updateUser(userEntity);
        verify(responseMapper).toDto(updatedUser);
    }

    @Test
    void shouldReturnBadRequestWhenValidationFails() {
        String userId = "123";
        UserUpdateDto updateDto = new UserUpdateDto("", "invalid", "short");
        
        when(bindingResult.hasErrors()).thenReturn(true);
        
        FieldError fieldError = new FieldError("userUpdateDto", "name", "Name cannot be empty");
        when(bindingResult.getFieldErrors()).thenReturn(java.util.List.of(fieldError));

        ResponseEntity<?> response = controller.updateUser(userId, updateDto, bindingResult);

        assertEquals(HttpStatus.BAD_REQUEST, response.getStatusCode());
        assertTrue(response.getBody() instanceof Map);

        @SuppressWarnings("unchecked")
        Map<String, String> errors = (Map<String, String>) response.getBody();
        assertEquals(1, errors.size());
        assertEquals("Name cannot be empty", errors.get("name"));

        verify(validator).validate(updateDto, bindingResult);
        verifyNoInteractions(updateMapper, userCommandService, responseMapper);
    }

    @Test
    void shouldReturnNotFoundWhenUserDoesNotExist() {
        String userId = "999";
        UserUpdateDto updateDto = new UserUpdateDto("Non Existent", "nonexistent", "password");
        User userEntity = new User(userId, "Non Existent", "nonexistent", "password");

        when(bindingResult.hasErrors()).thenReturn(false);
        when(updateMapper.toEntity(updateDto)).thenReturn(userEntity);
        when(userCommandService.updateUser(userEntity)).thenReturn(null);

        ResponseEntity<?> response = controller.updateUser(userId, updateDto, bindingResult);

        assertEquals(HttpStatus.NOT_FOUND, response.getStatusCode());
        assertNull(response.getBody());

        verify(validator).validate(updateDto, bindingResult);
        verify(updateMapper).toEntity(updateDto);
        verify(userCommandService).updateUser(userEntity);
        verifyNoInteractions(responseMapper);
    }

    @Test
    void shouldHandleNullInputsInUpdate() {
        String userId = "123";
        
        when(bindingResult.hasErrors()).thenReturn(false);
        when(updateMapper.toEntity(null)).thenThrow(new IllegalArgumentException());

        assertThrows(IllegalArgumentException.class, () -> 
            controller.updateUser(userId, null, bindingResult));
    }
}
