using AI.DocumentIntelligence.Application.Abstractions.AI;
using AI.DocumentIntelligence.Application.Contracts.AI;
using AI.DocumentIntelligence.Domain.Common;

namespace AI.DocumentIntelligence.Infrastructure.AI.Providers;

/// <summary>
/// Stub <see cref="IAIProvider"/> for Anthropic Claude. Selectable via
/// <c>AI:ProviderName = "Anthropic"</c> in configuration. A full implementation backed by the
/// Anthropic SDK should replace this stub when Claude access is required.
/// </summary>
internal sealed class AnthropicProvider : IAIProvider
{
    /// <summary>Stable identifier used for provider selection and telemetry.</summary>
    public const string ProviderName = "Anthropic";

    /// <inheritdoc />
    public string Name => ProviderName;

    /// <inheritdoc />
    public Task<Result<AiCompletionResult>> CompleteAsync(
        AiCompletionRequest request,
        CancellationToken cancellationToken = default) =>
        Task.FromResult(
            Result.Failure<AiCompletionResult>(
                Error.Failure(
                    "Anthropic.NotConfigured",
                    "The Anthropic provider is not yet configured. Set AI:ProviderName to 'AzureOpenAI' or provide a full Anthropic implementation.")));
}
