package jalau.usersapi.presentation.controllers;

import jakarta.validation.Valid;

import jalau.usersapi.core.domain.entities.User;
import jalau.usersapi.core.domain.services.IUserCommandService;

import jalau.usersapi.presentation.dtos.UserCreateDto;
import jalau.usersapi.presentation.dtos.UserUpdateDto;
import jalau.usersapi.presentation.dtos.UserResponseDto;

import jalau.usersapi.presentation.mappers.UserCreateDtoToUserMapper;
import jalau.usersapi.presentation.mappers.UserUpdateDtoToUserMapper;
import jalau.usersapi.presentation.mappers.UserToUserResponseDtoMapper;
import jalau.usersapi.presentation.validators.UserUpdateDtoValidator;

import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;

import org.springframework.validation.BindingResult;

import org.springframework.security.core.userdetails.UsernameNotFoundException;

import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PatchMapping;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.context.annotation.Profile;

import java.util.HashMap;
import java.util.Map;

/**
 * Handles HTTP requests for creating users.
 * Connects the request (DTO) with the business logic (Service).
 */
@RestController
@RequestMapping("/api/v1/users")
@Profile("!migration")
public class UserCommandController {
    private final IUserCommandService userCommandService;
    private final UserCreateDtoToUserMapper createMapper;
    private final UserUpdateDtoToUserMapper updateMapper;
    private final UserToUserResponseDtoMapper responseMapper;
    private final UserUpdateDtoValidator validator;

    /**
     * Constructor for injecting required services and mappers.
     */
    public UserCommandController(
            final IUserCommandService userCommandService,
            final UserCreateDtoToUserMapper createMapper,
            final UserUpdateDtoToUserMapper updateMapper,
            final UserToUserResponseDtoMapper responseMapper,
            final UserUpdateDtoValidator validator
    ) {
        this.userCommandService = userCommandService;
        this.createMapper = createMapper;
        this.updateMapper = updateMapper;
        this.responseMapper = responseMapper;
        this.validator = validator;
    }

    /**
     * POST endpoint to create a new user.
     * <p>Receives a DTO, converts it to an entity, saves it, and returns a response DTO.</p>
     *
     * @param userCreateDto DTO with data for the user to be created.
     * @return 201 Created response with the data of the created user.
     */
    @PostMapping
    public ResponseEntity<UserResponseDto> createUser(@Valid @RequestBody UserCreateDto userCreateDto) {
        User user = createMapper.toEntity(userCreateDto);
        User createdUser = userCommandService.createUser(user);
        UserResponseDto userResponseDto = responseMapper.toDto(createdUser);

        return ResponseEntity.status(HttpStatus.CREATED).body(userResponseDto);
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<Void> deleteUser(@PathVariable String id){
        try{
            userCommandService.deleteUser(id);
            //200
            return ResponseEntity.status(HttpStatus.OK).build();
        }
        catch (UsernameNotFoundException e){
            //will throw the 404 NOT FOUND exception
            return ResponseEntity.notFound().build();

        }
    }

    /**
     * PATCH endpoint to update a user.
     * 
     * @param id String
     * @param userUpdateDto DTO with data for the user to be update.
     * @return 200 Created response with the data of the update user.
     */
    @PatchMapping("/{id}")
    public ResponseEntity<?> updateUser(
        @PathVariable final String id, 
        @Valid @RequestBody final UserUpdateDto userUpdateDto,
        BindingResult bindingResult
    ) {
        validator.validate(userUpdateDto, bindingResult);

        if (bindingResult.hasErrors()) {
            Map<String, String> validationErrors = new HashMap<>();
            bindingResult.getFieldErrors().forEach(err ->
                    validationErrors.put(err.getField(), err.getDefaultMessage()));
            return ResponseEntity.badRequest().body(validationErrors);
        }
        
        User newUser = updateMapper.toEntity(userUpdateDto);
        newUser.setId(id);

        User updatedUser = userCommandService.updateUser(newUser);
        
        // 404 Not Found
        if (updatedUser == null) {
            return ResponseEntity.status(HttpStatus.NOT_FOUND).build();
        }

        UserResponseDto userResponseDto = responseMapper.toDto(updatedUser);

        return ResponseEntity.ok(userResponseDto);
    }
}