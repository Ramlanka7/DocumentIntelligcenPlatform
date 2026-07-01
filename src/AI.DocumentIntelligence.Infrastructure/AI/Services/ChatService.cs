using System.Diagnostics;
using AI.DocumentIntelligence.Application.Abstractions.AI;
using AI.DocumentIntelligence.Application.Abstractions.Identity;
using AI.DocumentIntelligence.Application.Abstractions.Persistence;
using AI.DocumentIntelligence.Application.Abstractions.Search;
using AI.DocumentIntelligence.Application.Contracts.Chat;
using AI.DocumentIntelligence.Domain.Common;
using AI.DocumentIntelligence.Infrastructure.AI.Prompts;
using Microsoft.Extensions.Logging;

namespace AI.DocumentIntelligence.Infrastructure.AI.Services;

/// <summary>
/// Implements <see cref="IChatService"/> using RAG retrieval and the configured
/// <see cref="IAIProvider"/>. Every answer carries source citations. Token usage is persisted
/// after each call.
/// </summary>
internal sealed partial class ChatService : AiServiceBase, IChatService
{
    private const string OperationType = "Chat";
    private const int ContextChunks = 8;

    private readonly ICurrentUser _currentUser;
    private readonly ILogger<ChatService> _logger;

    public ChatService(
        IAIProvider provider,
        ISearchService searchService,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser,
        ILogger<ChatService> logger)
        : base(provider, searchService, unitOfWork, logger)
    {
        _currentUser = currentUser;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<Result<ChatResponse>> AskAsync(
        ChatRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Message))
        {
            return Result.Failure<ChatResponse>(Domain.Errors.DomainErrors.Chat.EmptyMessage);
        }

        if (request.DocumentIds.Count == 0)
        {
            return Result.Failure<ChatResponse>(Domain.Errors.DomainErrors.Chat.NoDocuments);
        }

        var stopwatch = Stopwatch.StartNew();

        LogStartingChat(_logger, request.SessionId);

        var context = await RetrieveContextAsync(
            request.Message, request.DocumentIds, ContextChunks, cancellationToken);

        var (systemPrompt, userPrompt) = PromptTemplates.BuildChatPrompt(request.Message, context);

        var completionResult = await CompleteAsync(systemPrompt, userPrompt, cancellationToken);

        if (completionResult.IsFailure)
        {
            return Result.Failure<ChatResponse>(completionResult.Error);
        }

        var parseResult = ParseJson<JsonChatResponseDto>(completionResult.Value.Content);

        if (parseResult.IsFailure)
        {
            return Result.Failure<ChatResponse>(parseResult.Error);
        }

        var dto = parseResult.Value;
        var chatResponse = new ChatResponse(
            dto.Answer,
            MapCitations(dto.Citations),
            completionResult.Value.Usage);

        stopwatch.Stop();

        if (_currentUser.UserId.HasValue)
        {
            await TrackUsageAsync(
                _currentUser.UserId.Value,
                OperationType,
                completionResult.Value.Usage,
                stopwatch.Elapsed,
                request.SessionId,
                cancellationToken);
        }

        LogChatCompleted(_logger, request.SessionId, stopwatch.ElapsedMilliseconds);

        return Result.Success(chatResponse);
    }

    [LoggerMessage(Level = LogLevel.Information,
        Message = "Starting chat answer for session {SessionId}")]
    private static partial void LogStartingChat(ILogger logger, Guid sessionId);

    [LoggerMessage(Level = LogLevel.Information,
        Message = "Chat answer for session {SessionId} completed in {ElapsedMs} ms")]
    private static partial void LogChatCompleted(ILogger logger, Guid sessionId, long elapsedMs);
}
