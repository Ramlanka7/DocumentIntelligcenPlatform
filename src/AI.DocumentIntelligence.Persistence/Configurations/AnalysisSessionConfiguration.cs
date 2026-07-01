using AI.DocumentIntelligence.Domain.Entities;
using AI.DocumentIntelligence.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AI.DocumentIntelligence.Persistence.Configurations;

internal sealed class AnalysisSessionConfiguration : IEntityTypeConfiguration<AnalysisSession>
{
    public void Configure(EntityTypeBuilder<AnalysisSession> builder)
    {
        builder.ToTable("analysis_sessions");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasColumnName("id");

        builder.Property(s => s.OwnerId)
            .HasColumnName("owner_id")
            .IsRequired();

        builder.Property(s => s.Capability)
            .HasColumnName("capability")
            .HasConversion<string>()
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(s => s.CustomQuestion)
            .HasColumnName("custom_question");

        builder.Property(s => s.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(s => s.ExecutiveSummary)
            .HasColumnName("executive_summary");

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
        builder.Property<List<Guid>>(AnalysisSessionFieldNames.DocumentIds)
            .HasColumnName("document_ids")
            .HasColumnType("jsonb")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Property<List<string>>(AnalysisSessionFieldNames.KeyFindings)
            .HasColumnName("key_findings")
            .HasColumnType("jsonb")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Property<List<string>>(AnalysisSessionFieldNames.RisksIdentified)
            .HasColumnName("risks_identified")
            .HasColumnType("jsonb")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Property<List<string>>(AnalysisSessionFieldNames.Recommendations)
            .HasColumnName("recommendations")
            .HasColumnType("jsonb")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Property<List<string>>(AnalysisSessionFieldNames.ActionItems)
            .HasColumnName("action_items")
            .HasColumnType("jsonb")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Property<List<Citation>>(AnalysisSessionFieldNames.ReferencedSources)
            .HasColumnName("referenced_sources")
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
            .HasDatabaseName("ix_analysis_sessions_owner_id");

        builder.HasIndex(s => s.Status)
            .HasDatabaseName("ix_analysis_sessions_status");

        // Ignore computed read-only collection properties that are backed by the persisted fields.
        builder.Ignore(s => s.DocumentIds);
        builder.Ignore(s => s.KeyFindings);
        builder.Ignore(s => s.RisksIdentified);
        builder.Ignore(s => s.Recommendations);
        builder.Ignore(s => s.ActionItems);
        builder.Ignore(s => s.ReferencedSources);

        // Ignore domain events collection (not persisted).
        builder.Ignore(s => s.DomainEvents);
    }
}
