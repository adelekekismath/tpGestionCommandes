namespace Api.ViewModel.DTOs;

public record UserCreateDto(
    string Username,
    string Password,
    string Email
);

public record UserDto(
    string Id,
    string Username
);

public record UserUpdateDto(
    string? Password
);
public record LoginRequest(string Username, string Password);
public record LoginResponse(string AccessToken,  DateTime ExpireAt);
public record RegisterResponse(string Id, UserDto User);