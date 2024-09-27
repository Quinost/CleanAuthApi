namespace CleanAuth.Infrastructure.Interfaces;

public interface IJwtBlackList
{
    void AddToken(string token, DateTime date);
    bool IsTokenOnBlackList(string token);
    void RemoveExpiredToken();
}
