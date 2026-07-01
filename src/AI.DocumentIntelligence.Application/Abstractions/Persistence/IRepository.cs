using AI.DocumentIntelligence.Domain.Common;

namespace AI.DocumentIntelligence.Application.Abstractions.Persistence;

/// <summary>
/// Generic repository exposing standard CRUD operations and a queryable surface for all aggregate roots.
/// Implementations live in the Persistence layer; this interface is declared in Application to maintain
/// the Clean Architecture dependency direction.
/// </summary>
public interface IRepository<T> where T : BaseEntity
{
    public Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    public Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default);

    public Task AddAsync(T entity, CancellationToken cancellationToken = default);

    public void Update(T entity);

    public void Remove(T entity);

    public IQueryable<T> Query();
}
