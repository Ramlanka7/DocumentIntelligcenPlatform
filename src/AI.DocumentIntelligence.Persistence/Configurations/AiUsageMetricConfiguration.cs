using AI.DocumentIntelligence.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AI.DocumentIntelligence.Persistence.Configurations;

internal sealed class AiUsageMetricConfiguration : IEntityTypeConfiguration<AiUsageMetric>
{
    public void Configure(EntityTypeBuilder<AiUsageMetric> builder)
    {
        builder.ToTable("ai_usage_metrics");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .HasColumnName("id");

        builder.Property(m => m.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(m => m.OperationType)
            .HasColumnName("operation_type")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(m => m.SessionId)
            .HasColumnName("session_id");

        builder.Property(m => m.ProcessingTime)
            .HasColumnName("processing_time")
            .IsRequired();

        builder.Property(m => m.CreatedAtUtc)
            .HasColumnName("created_at_utc")
            .IsRequired();

        builder.Property(m => m.UpdatedAtUtc)
            .HasColumnName("updated_at_utc");

        // TokenUsage owned type.
        builder.OwnsOne(m => m.TokenUsage, tu =>
        {
            tu.Property(t => t.PromptTokens).HasColumnName("prompt_tokens").IsRequired();
            tu.Property(t => t.CompletionTokens).HasColumnName("completion_tokens").IsRequired();
            tu.Property(t => t.EstimatedCost).HasColumnName("estimated_cost").HasPrecision(18, 6).IsRequired();
        });

        builder.HasIndex(m => m.UserId)
            .HasDatabaseName("ix_ai_usage_metrics_user_id");

        builder.HasIndex(m => m.CreatedAtUtc)
            .HasDatabaseName("ix_ai_usage_metrics_created_at_utc");

        // Ignore domain events collection (not persisted).
        builder.Ignore(m => m.DomainEvents);
    }
}
