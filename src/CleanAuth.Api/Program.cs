using CleanAuth.Infrastructure;
using CleanAuth.Infrastructure.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace CleanAuth.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();

        Authorization(builder);

        builder.Services.AddInfrastructureServices(builder.Configuration);

        var app = builder.Build();

        // Configure the HTTP request pipeline.

        app.UseHttpsRedirection();

        app.AddBlacklistMiddleware();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MigrateDatabase();

        app.MapControllers();

        app.MapGet("/", (IConfiguration configuration) => $"Api works! {DateTime.UtcNow:yyyy-MM-dd HH:mm}");

        app.MapHealthChecks("/health");

        app.Run();
    }

    private static void Authorization(WebApplicationBuilder builder)
    {
        JwtConfig jwtConfig = GetJwtConfig(builder);

        var rsa = RSA.Create();
        rsa.ImportFromPem(jwtConfig.PublicKeyPEM);

        builder.Services.AddSingleton(jwtConfig);

        builder.Services.AddAuthentication(v =>
        {
            v.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            v.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(v =>
        {
            v.RequireHttpsMetadata = true;
            v.SaveToken = true;
            v.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = jwtConfig.Issuer,
                IssuerSigningKey = new RsaSecurityKey(rsa),
                ValidAudience = jwtConfig.Audience,
                ClockSkew = TimeSpan.Zero
            };
        });
    }

    private static JwtConfig GetJwtConfig(WebApplicationBuilder builder)
    {
        var jwtConfig = builder.Configuration.GetSection("JwtCfg").Get<JwtConfig>()!;
        var publicKeyPEM = File.ReadAllText(Directory.GetCurrentDirectory() + jwtConfig.PublicKeyPEM);
        var privateKeyPEM = File.ReadAllText(Directory.GetCurrentDirectory() + jwtConfig.PrivateKeyPEM);

        return jwtConfig with
        {
            PublicKeyPEM = publicKeyPEM,
            PrivateKeyPEM = privateKeyPEM
        };
    }
}
