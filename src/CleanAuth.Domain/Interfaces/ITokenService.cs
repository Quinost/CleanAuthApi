using Clean.Shared;
using CleanAuth.Domain.Dtos.Auth;

namespace CleanAuth.Domain.Interfaces;
public interface ITokenService
{
    Task<Result<TokenResultDto>> Login(string username, string password, CancellationToken token = default);
    Result Logout(string accessToken);
}
