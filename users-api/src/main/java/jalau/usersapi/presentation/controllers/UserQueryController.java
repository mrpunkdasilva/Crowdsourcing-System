package jalau.usersapi.presentation.controllers;

import java.util.List;

import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;
import org.springframework.context.annotation.Profile;

import jalau.usersapi.core.domain.entities.User;
import jalau.usersapi.core.domain.services.IUserQueryService;
import jalau.usersapi.presentation.dtos.UserResponseDto;
import jalau.usersapi.presentation.mappers.UserToUserResponseDtoMapper;

@RestController
@RequestMapping("/api/v1/users")
@Profile("!migration")
class UserQueryController {
    private final IUserQueryService userQueryService;
    private final UserToUserResponseDtoMapper mapper;

    UserQueryController(final IUserQueryService userQueryService,
                               final UserToUserResponseDtoMapper mapper) {
        this.userQueryService = userQueryService;
        this.mapper = mapper;
    }

    @GetMapping
    public ResponseEntity<List<UserResponseDto>> getUsers() {
        List<User> users = userQueryService.getUsers();
        
        List<UserResponseDto> dtos = users.stream()
                .map(mapper::toDto)
                .toList();
            
        return ResponseEntity.ok(dtos);
    }

    @GetMapping("/{id}")
    public ResponseEntity<List<UserResponseDto>> getUser(@PathVariable final String id) {
        List<User> user = userQueryService.getUser(id);
        System.out.println("Id: " + id);
        List<UserResponseDto> dtos = user.stream()
                .map(mapper::toDto)
                .toList();
            
        return ResponseEntity.ok(dtos);
    }

}
