using CleanAuth.Domain.Entities;
using CleanAuth.Domain.Interfaces;
using CleanAuth.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

namespace CleanAuth.Infrastructure.Repositories;
internal sealed class UserRepository(CleanDbContext dbContext) : BaseRepository<User>(dbContext), IUserRepository
{
    public override Task<User?> GetByIdAsync(Guid id, CancellationToken token = default)
        => dbContext.Users.Include(x => x.Role).FirstOrDefaultAsync(x => x.Id == id, token);

    public override async Task<ICollection<User>> GetAllPaginatedAsync(string filter,
                                                                       int pageNumber,
                                                                       int pageSize,
                                                                       CancellationToken token = default)
    {
        return await dbContext.Users
                .AsNoTracking()
                .Include(x => x.Role)
                .WhereIf(!string.IsNullOrEmpty(filter), x => x.Username.Contains(filter))
                .Pagination(pageNumber, pageSize)
                .ToListAsync(token);
    }

    public Task<User?> GetByUsernameAsync(string username, CancellationToken token = default)
        => dbContext.Users.FirstOrDefaultAsync(x => x.Username == username, token);

    public Task<bool> CheckIfUsernameIsUsedAsync(string username, CancellationToken token = default)
        => dbContext.Users.AnyAsync(x => x.Username == username, token);
}
