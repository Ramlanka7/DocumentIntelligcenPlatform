using AI.DocumentIntelligence.Domain.Entities;
using AI.DocumentIntelligence.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AI.DocumentIntelligence.Persistence.Configurations;

internal sealed class ComparisonSessionConfiguration : IEntityTypeConfiguration<ComparisonSession>
{
    public void Configure(EntityTypeBuilder<ComparisonSession> builder)
    {
        builder.ToTable("comparison_sessions");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasColumnName("id");

        builder.Property(s => s.OwnerId)
            .HasColumnName("owner_id")
            .IsRequired();

        builder.Property(s => s.ComparisonType)
            .HasColumnName("comparison_type")
            .HasConversion<string>()
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(s => s.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(s => s.ExecutiveOverview)
            .HasColumnName("executive_overview");

        builder.Property(s => s.RiskAnalysis)
            .HasColumnName("risk_analysis");

        builder.Property(s => s.ProcessingTime)
            .HasColumnName("processing_time");

        builder.Property(s => s.FailureReason)
            .HasColumnName("failure_reason")
            .HasMaxLength(2048);

        builder.Property(s => s.CreatedAtUtc)
            .HasColumnName("created_at_utc")
            .IsRequired();

        builder.Property(s => s.UpdatedAtUtc)
            .HasColumnName("updated_at_utc");

        // Store private list collections as JSON columns.
        builder.Property<List<Guid>>("_documentIds")
            .HasColumnName("document_ids")
            .HasColumnType("jsonb")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Property<List<string>>("_keyDifferences")
            .HasColumnName("key_differences")
            .HasColumnType("jsonb")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Property<List<string>>("_recommendations")
            .HasColumnName("recommendations")
            .HasColumnType("jsonb")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Property<List<ChangeLogEntry>>("_detailedChangeLog")
            .HasColumnName("detailed_change_log")
            .HasColumnType("jsonb")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Property<List<Citation>>("_sourceCitations")
            .HasColumnName("source_citations")
            .HasColumnType("jsonb")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        // TokenUsage owned type.
        builder.OwnsOne(s => s.TokenUsage, tu =>
        {
            tu.Property(t => t.PromptTokens).HasColumnName("prompt_tokens").IsRequired();
            tu.Property(t => t.CompletionTokens).HasColumnName("completion_tokens").IsRequired();
            tu.Property(t => t.EstimatedCost).HasColumnName("estimated_cost").HasPrecision(18, 6).IsRequired();
        });

        builder.HasIndex(s => s.OwnerId)
            .HasDatabaseName("ix_comparison_sessions_owner_id");

        // Ignore computed read-only collection properties backed by persisted fields.
        builder.Ignore(s => s.DocumentIds);
        builder.Ignore(s => s.KeyDifferences);
        builder.Ignore(s => s.Recommendations);
        builder.Ignore(s => s.DetailedChangeLog);
        builder.Ignore(s => s.SourceCitations);

        // Ignore domain events collection (not persisted).
        builder.Ignore(s => s.DomainEvents);
    }
}
