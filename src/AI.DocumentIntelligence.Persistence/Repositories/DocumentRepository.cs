using AI.DocumentIntelligence.Application.Abstractions.Persistence;
using AI.DocumentIntelligence.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AI.DocumentIntelligence.Persistence.Repositories;

internal sealed class DocumentRepository : Repository<Document>, IDocumentRepository
{
    public DocumentRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    public async Task<IReadOnlyList<Document>> GetByOwnerAsync(
        Guid ownerId,
        CancellationToken cancellationToken = default) =>
        await DbSet
            .Where(d => d.OwnerId == ownerId)
            .OrderByDescending(d => d.CreatedAtUtc)
            .ToListAsync(cancellationToken);

    public async Task<Document?> GetWithChunksAsync(
        Guid documentId,
        CancellationToken cancellationToken = default) =>
        await DbSet
            .Include(d => d.Chunks)
            .FirstOrDefaultAsync(d => d.Id == documentId, cancellationToken);
}
