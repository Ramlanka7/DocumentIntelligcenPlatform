using System.Diagnostics;
using AI.DocumentIntelligence.Application.Abstractions.AI;
using AI.DocumentIntelligence.Application.Abstractions.Identity;
using AI.DocumentIntelligence.Application.Abstractions.Persistence;
using AI.DocumentIntelligence.Application.Abstractions.Search;
using AI.DocumentIntelligence.Application.Contracts.Analysis;
using AI.DocumentIntelligence.Domain.Common;
using AI.DocumentIntelligence.Infrastructure.AI.Prompts;
using Microsoft.Extensions.Logging;

namespace AI.DocumentIntelligence.Infrastructure.AI.Services;

/// <summary>
/// Implements <see cref="IAnalysisService"/> using RAG retrieval from <see cref="ISearchService"/>
/// and the configured <see cref="IAIProvider"/>. Results always carry citations.
/// Token usage and cost are persisted as <see cref="Domain.Entities.AiUsageMetric"/> after each call.
/// </summary>
internal sealed partial class AnalysisService : AiServiceBase, IAnalysisService
{
    private const string OperationType = "Analysis";
    private const int ContextChunks = 10;

    private readonly ICurrentUser _currentUser;
    private readonly ILogger<AnalysisService> _logger;

    public AnalysisService(
        IAIProvider provider,
        ISearchService searchService,
        IUnitOfWork unitOfWork,
        ICurrentUser currentUser,
        ILogger<AnalysisService> logger)
        : base(provider, searchService, unitOfWork, logger)
    {
        _currentUser = currentUser;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<Result<AnalysisResult>> AnalyzeAsync(
        AnalysisRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.DocumentIds.Count == 0)
        {
            return Result.Failure<AnalysisResult>(Domain.Errors.DomainErrors.Analysis.NoDocuments);
        }

        if (request.DocumentIds.Count > 4)
        {
            return Result.Failure<AnalysisResult>(Domain.Errors.DomainErrors.Analysis.TooManyDocuments);
        }

        var stopwatch = Stopwatch.StartNew();

        LogStartingAnalysis(_logger, request.Capability, request.DocumentIds.Count);

        var query = string.IsNullOrWhiteSpace(request.CustomQuestion)
            ? request.Capability
            : request.CustomQuestion;

        var context = await RetrieveContextAsync(
            query, request.DocumentIds, ContextChunks, cancellationToken);

        var (systemPrompt, userPrompt) = PromptTemplates.BuildAnalysisPrompt(
            request.Capability, request.CustomQuestion, context);

        var completionResult = await CompleteAsync(systemPrompt, userPrompt, cancellationToken);

        if (completionResult.IsFailure)
        {
            return Result.Failure<AnalysisResult>(completionResult.Error);
        }

        var parseResult = ParseJson<JsonAnalysisResultDto>(completionResult.Value.Content);

        if (parseResult.IsFailure)
        {
            return Result.Failure<AnalysisResult>(parseResult.Error);
        }

        var dto = parseResult.Value;

        var analysisResult = new AnalysisResult(
            dto.ExecutiveSummary,
            dto.KeyFindings.Select(MapKeyFinding).ToList(),
            dto.Risks.Select(MapRiskItem).ToList(),
            dto.Recommendations.Select(MapRecommendation).ToList(),
            dto.ActionItems.Select(MapActionItem).ToList(),
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

        LogAnalysisCompleted(_logger, request.Capability, stopwatch.ElapsedMilliseconds);

        return Result.Success(analysisResult);
    }

    [LoggerMessage(Level = LogLevel.Information,
        Message = "Starting {Capability} analysis for {DocumentCount} document(s)")]
    private static partial void LogStartingAnalysis(
        ILogger logger, string capability, int documentCount);

    [LoggerMessage(Level = LogLevel.Information,
        Message = "Analysis '{Capability}' completed in {ElapsedMs} ms")]
    private static partial void LogAnalysisCompleted(
        ILogger logger, string capability, long elapsedMs);
}
