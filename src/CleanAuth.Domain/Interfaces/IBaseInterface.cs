namespace CleanAuth.Domain.Interfaces;
public interface IBaseInterface<TEntity> where TEntity : class
{
    abstract Task<TEntity?> GetByIdAsync(Guid id, CancellationToken token = default);

    abstract Task<ICollection<TEntity>> GetAllPaginatedAsync(string filter, int pageNumber, int pageSize, CancellationToken token = default);

    Task DeleteAsync(TEntity entity, CancellationToken token = default);

    Task AddAsync(TEntity entity);

    Task UpdateAsync(TEntity entity);
}
