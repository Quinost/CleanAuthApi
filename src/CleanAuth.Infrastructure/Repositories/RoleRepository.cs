using CleanAuth.Domain.Entities;
using CleanAuth.Domain.Interfaces;
using CleanAuth.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

namespace CleanAuth.Infrastructure.Repositories;
internal sealed class RoleRepository(CleanDbContext dbContext) : BaseRepository<Role>(dbContext), IRoleRepository
{
    public Task<bool> IsNameUsedAsync(string name, CancellationToken token = default)
    {
        return dbContext.Roles
            .AnyAsync(x => x.Name == name, token);
    }

    public override Task<Role?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        return dbContext.Roles
            .Include(x => x.Users)
            .FirstOrDefaultAsync(x => x.Id == id, token);
    }

    public override async Task<ICollection<Role>> GetAllPaginatedAsync(string filter, int pageNumber, int pageSize, CancellationToken token = default)
    {
        return await dbContext.Roles
                .AsNoTracking()
                .WhereIf(!string.IsNullOrEmpty(filter), x => x.Name.Contains(filter))
                .Pagination(pageNumber, pageSize)
                .ToListAsync(token);
    }
}
