using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using CleanAuth.Infrastructure.EF;
using CleanAuth.Infrastructure.Auth;
using CleanAuth.Domain.Interfaces;
using CleanAuth.Application;
using CleanAuth.Infrastructure.Workers;
using CleanAuth.Infrastructure.Interfaces;
using CleanAuth.Infrastructure.Repositories;
using CleanAuth.Infrastructure.Infra;

namespace CleanAuth.Infrastructure;

public static class InfrastructureExtension
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultDatabase")!;

        services.AddDbContext<CleanDbContext>(x => x.UseSqlServer(connectionString));
        services.AddHealthChecks().AddSqlServer(connectionString);

        services.AddMediatRHandlers();

        services.AddControllers(options =>
        {
            options.Filters.Add<FluentValidationActionFilter>();
        });

        services.AddHostedService<BlackListExpiredWorker>();

        services.AddSingleton<IJwtBlackList, JwtBlackList>();

        services.AddTransient<ITokenService, TokenService>();
        services.AddTransient<IUserManager, UserManager>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRsaKeyRepository, RsaKeyRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();

        return services;
    }

    public static IApplicationBuilder MigrateDatabase(this IApplicationBuilder app)
    {
        using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()!.CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetRequiredService<CleanDbContext>();
            context.Database.Migrate();
        }

        return app;
    }

    public static IApplicationBuilder AddBlacklistMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<BlackListMiddleware>();
        return app;
    }
}
