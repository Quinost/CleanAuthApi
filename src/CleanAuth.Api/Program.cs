using CleanAuth.Infrastructure;
using CleanAuth.Infrastructure.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

        app.UseAuthentication();

        app.UseAuthorization();

        app.AddInstrastructureApplications();

        app.MapControllers();

        app.MapGet("/", (IConfiguration configuration) => $"Api works! {DateTime.Now:yyyy-MM-dd HH:mm} - {configuration.GetConnectionString("DefaultDatabase")}");

        app.Run();
    }

    private static void Authorization(WebApplicationBuilder builder)
    {
        var jwtConfig = builder.Configuration.GetSection("JwtCfg").Get<JwtConfig>()!;

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
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConfig.Secret)),
                ValidAudience = jwtConfig.Audience,
                ClockSkew = TimeSpan.Zero
            };
        });
    }
}
