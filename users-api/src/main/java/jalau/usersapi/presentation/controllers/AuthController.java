package jalau.usersapi.presentation.controllers;

import jalau.usersapi.core.domain.entities.User;
import jalau.usersapi.core.domain.services.IAuthQueryService;
import jalau.usersapi.presentation.dtos.UserResponseDto;
import jalau.usersapi.presentation.mappers.UserToUserResponseDtoMapper;
import org.springframework.context.annotation.Profile;
import org.springframework.http.HttpHeaders;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestHeader;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

@RestController
@RequestMapping("/api/v1/auth")
@Profile("!migration")
public class AuthController {

    private final IAuthQueryService authQueryService;
    private final UserToUserResponseDtoMapper userToUserResponseDtoMapper;

    public AuthController(IAuthQueryService authQueryService, UserToUserResponseDtoMapper userToUserResponseDtoMapper) {
        this.authQueryService = authQueryService;
        this.userToUserResponseDtoMapper = userToUserResponseDtoMapper;
    }

    @PostMapping
    public ResponseEntity<UserResponseDto> authorize(@RequestHeader(HttpHeaders.AUTHORIZATION) String authorizationHeader) {
        User user = authQueryService.authorize(authorizationHeader);

        if (user != null) {
            return ResponseEntity.ok(userToUserResponseDtoMapper.toDto(user));
        }

        return ResponseEntity.notFound().build();
    }
}
