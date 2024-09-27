using CleanAuth.Infrastructure.Auth;
using CleanAuth.Infrastructure.Interfaces;
using Microsoft.Extensions.Hosting;

namespace CleanAuth.Infrastructure.Workers;

internal sealed class BlackListExpiredWorker(IJwtBlackList jwtBlackList, JwtConfig jwtConfig) : BackgroundService
{
    private readonly int WorkerDelay = jwtConfig.JwtBlackListWorkerDelayInHours;
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        jwtBlackList.RemoveExpiredToken();
        return Task.Delay(TimeSpan.FromHours(WorkerDelay), stoppingToken);
    }
}
