using System.Collections.Concurrent;
using CleanAuth.Infrastructure.Interfaces;

namespace CleanAuth.Infrastructure.Auth;

internal sealed class JwtBlackList : IJwtBlackList
{
    private ConcurrentDictionary<string, DateTime> AccessTokens { get; set; } = new ConcurrentDictionary<string, DateTime>();

    public void AddToken(string token, DateTime date)
    {
        var accessToken = ExtractToken(token);

        if (AccessTokens.ContainsKey(accessToken))
            return;

        AccessTokens.TryAdd(accessToken, date);
    }

    public bool IsTokenOnBlackList(string token)
    {
        var accessToken = ExtractToken(token);

        return AccessTokens.ContainsKey(accessToken);
    }

    public void RemoveExpiredToken()
    {
        var now = DateTime.UtcNow;
        foreach (var token in AccessTokens.Where(v => v.Value < now))
        {
            AccessTokens.TryRemove(token);
        }
    }

    private static string ExtractToken(string token)
    {
        string accessToken = token;

        if (token.Contains("Bearer", StringComparison.OrdinalIgnoreCase))
            accessToken = token.Split(' ', 2)[1];

        return accessToken;
    }
}
