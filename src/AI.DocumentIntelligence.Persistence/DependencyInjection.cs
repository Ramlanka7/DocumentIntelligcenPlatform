using AI.DocumentIntelligence.Application.Abstractions.Persistence;
using AI.DocumentIntelligence.Persistence.Repositories;
using AI.DocumentIntelligence.Persistence.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AI.DocumentIntelligence.Persistence;

/// <summary>
/// Registers all Persistence-layer services (DbContext, Unit of Work, repositories, seeder)
/// into the DI container. Called from the Api composition root.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                npgsql => npgsql.UseVector()));

        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Repositories
        services.AddScoped<IDocumentRepository, DocumentRepository>();
        services.AddScoped<IAnalysisSessionRepository, AnalysisSessionRepository>();
        services.AddScoped<IComparisonSessionRepository, ComparisonSessionRepository>();
        services.AddScoped<IChatSessionRepository, ChatSessionRepository>();
        services.AddScoped<IAuditLogRepository, AuditLogRepository>();
        services.AddScoped<IAiUsageMetricRepository, AiUsageMetricRepository>();

        // Seeder (transient — invoked once at startup, not injected into request pipeline)
        services.AddTransient<DataSeeder>();

        return services;
    }
}
