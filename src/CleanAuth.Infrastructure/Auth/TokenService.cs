using Clean.Shared;
using CleanAuth.Domain.Dtos.Auth;
using CleanAuth.Domain.Entities;
using CleanAuth.Domain.Interfaces;
using CleanAuth.Infrastructure.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace CleanAuth.Infrastructure.Auth;

internal sealed class TokenService(
    JwtConfig jwtConfig,
    IUserManager userManager,
    IJwtBlackList jwtBlackList) : ITokenService
{
    public async Task<Result<TokenResultDto>> Login(string username, string password, CancellationToken token = default)
    {
        var user = await userManager.FindByNameAsync(username, token);
        if (user == null)
            return Result.Failed("Invalid username or password");

        if (!user.IsActive)
            return Result.Failed("User is not active");

        if (!userManager.CheckPassword(user, password))
            return Result.Failed("Invalid username or passwordd");

        var tokenResult = GenerateToken(user);

        return Result.Ok(tokenResult);
    }

    public Result Logout(string accessToken)
    {
        var (_, JwtToken) = DecodeJwtToken(accessToken);
        jwtBlackList.AddToken(accessToken, JwtToken.ValidTo);
        return Result.Ok();
    }

    private TokenResultDto GenerateToken(User user)
    {
        var date = DateTime.UtcNow;
        var token = GenerateAccessToken(date, user.Username);

        return new TokenResultDto(token);
    }

    private string GenerateAccessToken(DateTime now, string username)
    {
        var expiry = now.AddMinutes(jwtConfig.AccessTokenExpiration);
        var claims = new[] { new Claim(ClaimTypes.Name, username) };

        var rsa = RSA.Create();
        rsa.ImportFromPem(jwtConfig.PrivateKeyPEM);

        var signingCredentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);

        var accessToken = new JwtSecurityToken(
            issuer: jwtConfig.Issuer,
            audience: jwtConfig.Audience,
            claims: claims,
            notBefore: now,
            expires: expiry,
            signingCredentials: signingCredentials
            );

        return new JwtSecurityTokenHandler().WriteToken(accessToken);
    }

    private (ClaimsPrincipal Claims, JwtSecurityToken JwtToken) DecodeJwtToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return default;

        var rsa = RSA.Create();
        rsa.ImportFromPem(jwtConfig.PublicKeyPEM);

        var principal = new JwtSecurityTokenHandler()
            .ValidateToken(token,
                new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = jwtConfig.Issuer,
                    IssuerSigningKey = new RsaSecurityKey(rsa),
                    ValidAudience = jwtConfig.Audience,
                    ClockSkew = TimeSpan.Zero
                },
                out var validatedToken);

        if (principal is null || validatedToken is not JwtSecurityToken securityToken)
            return default;

        return (principal, securityToken);
    }
}
