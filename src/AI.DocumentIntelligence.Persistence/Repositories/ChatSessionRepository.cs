using AI.DocumentIntelligence.Application.Abstractions.Persistence;
using AI.DocumentIntelligence.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AI.DocumentIntelligence.Persistence.Repositories;

internal sealed class ChatSessionRepository : Repository<ChatSession>, IChatSessionRepository
{
    public ChatSessionRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    public async Task<ChatSession?> GetWithMessagesAsync(
        Guid sessionId,
        CancellationToken cancellationToken = default) =>
        await DbSet
            .Include(s => s.Messages)
            .FirstOrDefaultAsync(s => s.Id == sessionId, cancellationToken);
}
