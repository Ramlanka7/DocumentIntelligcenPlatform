namespace AI.DocumentIntelligence.Application.Abstractions.Persistence;

/// <summary>
/// Encapsulates the save-changes boundary for a unit of work backed by EF Core.
/// The implementation wraps <c>ApplicationDbContext.SaveChangesAsync</c> and lives
/// in the Persistence layer.
/// </summary>
public interface IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
