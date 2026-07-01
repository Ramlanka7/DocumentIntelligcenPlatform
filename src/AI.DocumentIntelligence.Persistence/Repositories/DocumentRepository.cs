using System.Linq.Expressions;
using AI.DocumentIntelligence.Application.Abstractions.Persistence;
using AI.DocumentIntelligence.Domain.Entities;

namespace AI.DocumentIntelligence.Persistence.Repositories;

/// <summary>
/// Stub implementation of <see cref="IDocumentRepository"/>.
/// A full EF Core implementation will replace this when T02 (Persistence) is complete.
/// </summary>
public sealed class DocumentRepository : IDocumentRepository
{
    // T02 is not yet done; all methods throw NotImplementedException with a clear message.

    public Task<Document?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        throw new NotImplementedException("DocumentRepository is a stub pending T02 (EF Core Persistence).");

    public Task<IReadOnlyList<Document>> GetAllAsync(CancellationToken cancellationToken = default) =>
        throw new NotImplementedException("DocumentRepository is a stub pending T02 (EF Core Persistence).");

    public Task<IReadOnlyList<Document>> FindAsync(
        Expression<Func<Document, bool>> predicate,
        CancellationToken cancellationToken = default) =>
        throw new NotImplementedException("DocumentRepository is a stub pending T02 (EF Core Persistence).");

    public Task AddAsync(Document entity, CancellationToken cancellationToken = default) =>
        throw new NotImplementedException("DocumentRepository is a stub pending T02 (EF Core Persistence).");

    public void Update(Document entity) =>
        throw new NotImplementedException("DocumentRepository is a stub pending T02 (EF Core Persistence).");

    public void Remove(Document entity) =>
        throw new NotImplementedException("DocumentRepository is a stub pending T02 (EF Core Persistence).");

    public Task<IReadOnlyList<Document>> GetByOwnerAsync(Guid ownerId, CancellationToken ct = default) =>
        throw new NotImplementedException("DocumentRepository is a stub pending T02 (EF Core Persistence).");
}
