using CleanAuth.Domain.Entities;

namespace CleanAuth.Domain.Interfaces;
public interface IUserRepository
{
    Task AddUserAsync(User user);
    Task DeleteUserAsync(User user, CancellationToken token = default);
    Task<User?> GetUserByIdAsync(Guid id, CancellationToken token = default);
    Task<ICollection<User>> GetUsersAsync(string filter, int pageNumber, int pageSize);
    Task UpdateAsync(User user);
    Task<bool> CheckIfUsernameIsUsed(string username, CancellationToken token = default);
    Task<User?> GetByUsernameAsync(string username, CancellationToken token = default);
}