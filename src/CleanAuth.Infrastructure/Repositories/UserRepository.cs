using CleanAuth.Domain.Entities;
using CleanAuth.Domain.Interfaces;
using CleanAuth.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

namespace CleanAuth.Infrastructure.Repositories;
internal sealed class UserRepository(CleanDbContext dbContext) : BaseRepository<User>(dbContext), IUserRepository
{
    public override Task<User?> GetUserByIdAsync(Guid id, CancellationToken token = default) 
        => dbContext.Users.FirstOrDefaultAsync(x => x.Id == id, token);

    public async Task<ICollection<User>> GetUsersAsync(string filter, int pageNumber, int pageSize)
    {
        return await dbContext.Users
                .WhereIf(!string.IsNullOrEmpty(filter), x => x.Username.Contains(filter))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
    }

    public Task<User?> GetByUsernameAsync(string username, CancellationToken token = default) 
        => dbContext.Users.FirstOrDefaultAsync(x => x.Username == username, token);

    public Task<bool> CheckIfUsernameIsUsed(string username, CancellationToken token = default) 
        => dbContext.Users.AnyAsync(x => x.Username == username, token);
}
