using CleanAuth.Domain.Entities;

namespace CleanAuth.Domain.Interfaces;
public interface IUserRepository : IBaseInterface<User>
{
    Task<bool> CheckIfUsernameIsUsedAsync(string username, CancellationToken token = default);
    Task<User?> GetByUsernameAsync(string username, CancellationToken token = default);
}