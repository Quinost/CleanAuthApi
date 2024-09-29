using CleanAuth.Domain.Entities;

namespace CleanAuth.Infrastructure.Interfaces;
internal interface IUserManager
{
    void Activate(User user);
    bool CheckPassword(User user, string password);
    void Deactivate(User user);
    Task<User?> FindByNameAsync(string username, CancellationToken token = default);
    Task<bool> UpdateUserAsync(User user);
}