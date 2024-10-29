namespace CleanAuth.Domain.Dtos.Users;

public record UserDto(
    Guid Id,
    string Username,
    bool IsActive,
    Guid RoleId,
    string RoleName);
