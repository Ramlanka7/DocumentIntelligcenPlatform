using AI.DocumentIntelligence.Domain.Entities;

namespace AI.DocumentIntelligence.Application.Abstractions.Persistence;

/// <summary>Repository for <see cref="Document"/> aggregates with document-specific queries.</summary>
public interface IDocumentRepository : IRepository<Document>
{
    /// <summary>Returns all documents owned by the specified user.</summary>
    public Task<IReadOnlyList<Document>> GetByOwnerAsync(Guid ownerId, CancellationToken cancellationToken = default);

    /// <summary>Returns a document with its chunks eagerly loaded, or <see langword="null"/> if not found.</summary>
    public Task<Document?> GetWithChunksAsync(Guid documentId, CancellationToken cancellationToken = default);
}
