using AI.DocumentIntelligence.Domain.Entities;
using AI.DocumentIntelligence.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AI.DocumentIntelligence.Persistence.Configurations;

internal sealed class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage>
{
    public void Configure(EntityTypeBuilder<ChatMessage> builder)
    {
        builder.ToTable("chat_messages");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .HasColumnName("id");

        builder.Property(m => m.ChatSessionId)
            .HasColumnName("chat_session_id")
            .IsRequired();

        builder.Property(m => m.Ordinal)
            .HasColumnName("ordinal")
            .IsRequired();

        builder.Property(m => m.Role)
            .HasColumnName("role")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(m => m.Content)
            .HasColumnName("content")
            .IsRequired();

        builder.Property(m => m.CreatedAtUtc)
            .HasColumnName("created_at_utc")
            .IsRequired();

        builder.Property(m => m.UpdatedAtUtc)
            .HasColumnName("updated_at_utc");

        // Store private _citations list as a JSON column.
        builder.Property<List<Citation>>("_citations")
            .HasColumnName("citations")
            .HasColumnType("jsonb")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        // TokenUsage owned type.
        builder.OwnsOne(m => m.TokenUsage, tu =>
        {
            tu.Property(t => t.PromptTokens).HasColumnName("prompt_tokens").IsRequired();
            tu.Property(t => t.CompletionTokens).HasColumnName("completion_tokens").IsRequired();
            tu.Property(t => t.EstimatedCost).HasColumnName("estimated_cost").HasPrecision(18, 6).IsRequired();
        });

        builder.HasIndex(m => m.ChatSessionId)
            .HasDatabaseName("ix_chat_messages_chat_session_id");

        builder.HasIndex(m => new { m.ChatSessionId, m.Ordinal })
            .HasDatabaseName("ix_chat_messages_session_ordinal");

        // Ignore computed read-only collection backed by persisted field.
        builder.Ignore(m => m.Citations);

        // Ignore domain events collection (not persisted).
        builder.Ignore(m => m.DomainEvents);
    }
}
