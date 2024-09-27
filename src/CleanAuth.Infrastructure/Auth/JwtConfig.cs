using System.Text.Json.Serialization;

namespace CleanAuth.Infrastructure.Auth;

public sealed class JwtConfig
{
    [JsonPropertyName("Secret")]
    public required string Secret { get; set; }

    [JsonPropertyName("Issuer")]
    public required string Issuer { get; set; }

    [JsonPropertyName("Audience")]
    public required string Audience { get; set; }

    [JsonPropertyName("AccessTokenExpiration")]
    public required int AccessTokenExpiration { get; set; }

    [JsonPropertyName("RefreshTokenExpiration")]
    public required int RefreshTokenExpiration { get; set; }

    [JsonPropertyName("JwtBlackListWorkerDelayInHours")]
    public required int JwtBlackListWorkerDelayInHours { get; set; }
}