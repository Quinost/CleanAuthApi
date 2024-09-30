namespace CleanAuth.Infrastructure.Auth;

public record JwtConfig(
    string PublicKeyPEM,
    string PrivateKeyPEM,
    string Issuer,
    string Audience,
    int AccessTokenExpiration,
    int RefreshTokenExpiration,
    int JwtBlackListWorkerDelayInHours);