namespace AI.DocumentIntelligence.Infrastructure.AI.Options;

/// <summary>
/// Configuration options for Azure OpenAI (Foundry). Bound from the
/// <c>AzureOpenAI</c> section of <c>appsettings.json</c>.
/// </summary>
public sealed class AzureOpenAIOptions
{
    public const string SectionName = "AzureOpenAI";

    /// <summary>The Azure OpenAI service endpoint URI (e.g. https://&lt;name&gt;.openai.azure.com/).</summary>
    public string Endpoint { get; set; } = string.Empty;

    /// <summary>The Azure OpenAI API key.</summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>The deployment (model) name to use for chat completions (e.g. "gpt-4o").</summary>
    public string ChatDeployment { get; set; } = "gpt-4o";

    /// <summary>The deployment name to use for embeddings (e.g. "text-embedding-3-small").</summary>
    public string EmbeddingDeployment { get; set; } = "text-embedding-3-small";

    /// <summary>The expected embedding vector dimensionality (must match the deployment model).</summary>
    public int EmbeddingDimensions { get; set; } = 1536;
}
