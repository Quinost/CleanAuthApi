global using MediatR;
global using Clean.Shared;



using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CleanAuth.Application;

public static class MediatorDependencyInjection
{
    public static IServiceCollection AddMediatRHandlers(this IServiceCollection services)
    {
        services.AddMediatR(opt => opt.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        return services;
    }
}
