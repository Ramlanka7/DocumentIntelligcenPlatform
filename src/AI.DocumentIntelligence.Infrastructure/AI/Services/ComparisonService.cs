using System.Diagnostics;
using AI.DocumentIntelligence.Application.Abstractions.AI;
using AI.DocumentIntelligence.Application.Abstractions.Identity;
using AI.DocumentIntelligence.Application.Abstractions.Persistence;
using AI.DocumentIntelligence.Application.Abstractions.Search;
using AI.DocumentIntelligence.Application.Contracts.Comparison;
using AI.DocumentIntelligence.Domain.Common;
using AI.DocumentIntelligence.Infrastructure.AI.Prompts;
using Microsoft.Extensions.Logging;

namespace AI.DocumentIntelligence.Infrastructure.AI.Services;

/// <summary>
/// Implements <see cref="IComparisonService"/> using RAG retrieval and the configured
/// <see cref="IAIProvider"/>. Identifies added, removed, and modified content across 2–4 documents.
/// Results always carry citations. Token usage is persisted after each call.
/// </summary>
internal sealed partial class ComparisonService : AiServiceBase, IComparisonService
{
    private const string OperationType = "Comparison";
    private const int ContextChunks = 15;

    private readonly ICurrentUser _currentUser;
    private readonly ILogger<ComparisonService> _logger;

    public ComparisonService(
        IAIProvider provider,
        ISearchService searchService,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser,
        ILogger<ComparisonService> logger)
        : base(provider, searchService, unitOfWork, logger)
    {
        _currentUser = currentUser;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<Result<ComparisonResult>> CompareAsync(
        ComparisonRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.DocumentIds.Count < 2 || request.DocumentIds.Count > 4)
        {
            return Result.Failure<ComparisonResult>(
                Domain.Errors.DomainErrors.Comparison.InsufficientDocuments);
        }

        var stopwatch = Stopwatch.StartNew();

        LogStartingComparison(_logger, request.ComparisonType, request.DocumentIds.Count);

        var query = string.IsNullOrWhiteSpace(request.CustomInstructions)
            ? $"{request.ComparisonType} comparison"
            : request.CustomInstructions;

        var context = await RetrieveContextAsync(
            query, request.DocumentIds, ContextChunks, cancellationToken);

        var (systemPrompt, userPrompt) = PromptTemplates.BuildComparisonPrompt(
            request.ComparisonType, request.CustomInstructions, context);

        var completionResult = await CompleteAsync(systemPrompt, userPrompt, cancellationToken);

        if (completionResult.IsFailure)
        {
            return Result.Failure<ComparisonResult>(completionResult.Error);
        }

        var parseResult = ParseJson<JsonComparisonResultDto>(completionResult.Value.Content);

        if (parseResult.IsFailure)
        {
            return Result.Failure<ComparisonResult>(parseResult.Error);
        }

        var dto = parseResult.Value;

        var comparisonResult = new ComparisonResult(
            dto.ExecutiveOverview,
            dto.Differences.Select(MapDifference).ToList(),
            dto.Risks.Select(MapRiskItem).ToList(),
            dto.Recommendations.Select(MapRecommendation).ToList(),
            MapCitations(dto.Sources));

        stopwatch.Stop();

        if (_currentUser.UserId.HasValue)
        {
            await TrackUsageAsync(
                _currentUser.UserId.Value,
                OperationType,
                completionResult.Value.Usage,
                stopwatch.Elapsed,
                sessionId: null,
                cancellationToken);
        }

        LogComparisonCompleted(_logger, request.ComparisonType, stopwatch.ElapsedMilliseconds);

        return Result.Success(comparisonResult);
    }

    [LoggerMessage(Level = LogLevel.Information,
        Message = "Starting {ComparisonType} comparison for {DocumentCount} document(s)")]
    private static partial void LogStartingComparison(
        ILogger logger, string comparisonType, int documentCount);

    [LoggerMessage(Level = LogLevel.Information,
        Message = "Comparison '{ComparisonType}' completed in {ElapsedMs} ms")]
    private static partial void LogComparisonCompleted(
        ILogger logger, string comparisonType, long elapsedMs);
}
