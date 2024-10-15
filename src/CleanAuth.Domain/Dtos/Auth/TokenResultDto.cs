namespace CleanAuth.Domain.Dtos.Auth;
public record TokenResultDto(
    string AccessToken,
    DateTime ExpirationDateUTC);
