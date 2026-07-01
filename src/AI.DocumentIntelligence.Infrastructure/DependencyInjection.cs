using AI.DocumentIntelligence.Application.Abstractions.AI;
using AI.DocumentIntelligence.Application.Abstractions.Documents;
using AI.DocumentIntelligence.Application.Abstractions.Search;
using AI.DocumentIntelligence.Infrastructure.AI.Embedding;
using AI.DocumentIntelligence.Infrastructure.AI.Options;
using AI.DocumentIntelligence.Infrastructure.AI.Search;
using AI.DocumentIntelligence.Infrastructure.Documents;
using AI.DocumentIntelligence.Infrastructure.Documents.Chunking;
using AI.DocumentIntelligence.Infrastructure.Documents.Processors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AI.DocumentIntelligence.Infrastructure;

/// <summary>
/// Wires all Infrastructure services into the DI container. Called once at API startup.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // ---- Document processors (T04) ----
        services.AddTransient<IDocumentProcessor, PdfDocumentProcessor>();
        services.AddTransient<IDocumentProcessor, WordDocumentProcessor>();
        services.AddTransient<IDocumentProcessor, TextDocumentProcessor>();
        services.AddTransient<IDocumentProcessor, CsvDocumentProcessor>();
        services.AddTransient<IDocumentProcessor, PowerPointDocumentProcessor>();

        services.AddTransient<IDocumentProcessorFactory, DocumentProcessorFactory>();

        // ---- Chunking (T05) ----
        services.Configure<ChunkingOptions>(
            configuration.GetSection(ChunkingOptions.SectionName));

        services.AddSingleton<IChunkingService, ChunkingService>();

        // ---- Azure OpenAI options + embedding (T05) ----
        services.Configure<AzureOpenAIOptions>(
            configuration.GetSection(AzureOpenAIOptions.SectionName));

        services.AddSingleton<IEmbeddingService, AzureOpenAIEmbeddingService>();

        // ---- Azure AI Search (T05) ----
        services.Configure<AzureSearchOptions>(
            configuration.GetSection(AzureSearchOptions.SectionName));

        services.AddSingleton<ISearchService, AzureSearchService>();

        return services;
    }
}
