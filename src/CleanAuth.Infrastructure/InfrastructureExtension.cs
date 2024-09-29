using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using CleanAuth.Infrastructure.EF;
using CleanAuth.Infrastructure.Auth;
using CleanAuth.Infrastructure.Middlewares;
using CleanAuth.Domain.Interfaces;
using CleanAuth.Application;
using CleanAuth.Infrastructure.Workers;
using CleanAuth.Infrastructure.Interfaces;
using CleanAuth.Infrastructure.Repositories;

namespace CleanAuth.Infrastructure;

public static class InfrastructureExtension
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddDbContext<CleanDbContext>(x => x.UseSqlServer(configuration.GetConnectionString("DefaultDatabase")));

        service.AddMediatRHandlers();

        service.AddHostedService<BlackListExpiredWorker>();

        service.AddSingleton<IJwtBlackList, JwtBlackList>();

        service.AddTransient<ITokenService, TokenService>();
        service.AddTransient<IUserManager, UserManager>();

        service.AddScoped<IUserRepository, UserRepository>();

        return service;
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
