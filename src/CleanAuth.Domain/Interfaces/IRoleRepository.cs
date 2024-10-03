using CleanAuth.Domain.Entities;

namespace CleanAuth.Domain.Interfaces;
public interface IRoleRepository : IBaseInterface<Role>
{
    Task<bool> IsNameUsedAsync(string name, CancellationToken token = default);
}