using AI.DocumentIntelligence.Application.Abstractions.Persistence;
using AI.DocumentIntelligence.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AI.DocumentIntelligence.Persistence.Repositories;

internal sealed class AnalysisSessionRepository : Repository<AnalysisSession>, IAnalysisSessionRepository
{
    public AnalysisSessionRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    public async Task<IReadOnlyList<AnalysisSession>> GetByDocumentAsync(
        Guid documentId,
        CancellationToken cancellationToken = default)
    {
        // _documentIds is stored as a JSON column; query using the EF field accessor.
        return await DbSet
            .Where(s => EF.Property<List<Guid>>(s, "_documentIds").Contains(documentId))
            .OrderByDescending(s => s.CreatedAtUtc)
            .ToListAsync(cancellationToken);
    }
}
