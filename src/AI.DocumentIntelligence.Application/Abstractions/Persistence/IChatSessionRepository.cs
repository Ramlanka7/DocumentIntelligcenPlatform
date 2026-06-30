using AI.DocumentIntelligence.Domain.Entities;

namespace AI.DocumentIntelligence.Application.Abstractions.Persistence;

/// <summary>Repository for <see cref="ChatSession"/> aggregates with session-specific queries.</summary>
public interface IChatSessionRepository : IRepository<ChatSession>
{
    /// <summary>Returns a chat session with its messages eagerly loaded, or <see langword="null"/> if not found.</summary>
    public Task<ChatSession?> GetWithMessagesAsync(Guid sessionId, CancellationToken cancellationToken = default);
}
