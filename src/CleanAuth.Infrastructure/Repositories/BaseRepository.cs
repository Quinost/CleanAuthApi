using CleanAuth.Infrastructure.EF;

namespace CleanAuth.Infrastructure.Repositories;
internal abstract class BaseRepository<TEntity>(CleanDbContext dbContext) where TEntity : class
{
    protected CleanDbContext dbContext = dbContext;

    public abstract Task<TEntity?> GetUserByIdAsync(Guid id, CancellationToken token = default);

    public async Task DeleteUserAsync(TEntity entity, CancellationToken token = default)
    {
        dbContext.Set<TEntity>().Remove(entity);
        await dbContext.SaveChangesAsync(token);
    }

    public async Task AddUserAsync(TEntity entity)
    {
        await dbContext.Set<TEntity>().AddAsync(entity);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(TEntity entity)
    {
        dbContext.Set<TEntity>().Update(entity);
        await dbContext.SaveChangesAsync();
    }
}
