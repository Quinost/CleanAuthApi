using CleanAuth.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace CleanAuth.Infrastructure.Middlewares;

internal sealed class BlackListMiddleware(RequestDelegate next, IJwtBlackList jwtBlackList)
{
    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue(HeaderNames.Authorization, out var authHeader))
            if (jwtBlackList.IsTokenOnBlackList(authHeader.ToString()))
                context.Request.Headers.Remove(HeaderNames.Authorization);

        await next(context);
    }
}
