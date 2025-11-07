namespace Api.ViewModel.DTOs;

public record UserCreateDto(
    string Username,
    string Password
);

public record UserDto(
    int Id,
    string Username
);

public record UserUpdateDto(
    string? Password
);