using AI.DocumentIntelligence.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AI.DocumentIntelligence.Persistence.Configurations;

internal sealed class ChatSessionConfiguration : IEntityTypeConfiguration<ChatSession>
{
    public void Configure(EntityTypeBuilder<ChatSession> builder)
    {
        builder.ToTable("chat_sessions");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasColumnName("id");

        builder.Property(s => s.OwnerId)
            .HasColumnName("owner_id")
            .IsRequired();

        builder.Property(s => s.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(s => s.CreatedAtUtc)
            .HasColumnName("created_at_utc")
            .IsRequired();

        builder.Property(s => s.UpdatedAtUtc)
            .HasColumnName("updated_at_utc");

        // Store private _documentIds list as a JSON column.
        builder.Property<List<Guid>>("_documentIds")
            .HasColumnName("document_ids")
            .HasColumnType("jsonb")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasIndex(s => s.OwnerId)
            .HasDatabaseName("ix_chat_sessions_owner_id");

        // One-to-many: ChatSession owns its messages.
        builder.HasMany(s => s.Messages)
            .WithOne()
            .HasForeignKey(m => m.ChatSessionId)
            .OnDelete(DeleteBehavior.Cascade);

        // Ignore computed read-only collection backed by persisted field.
        builder.Ignore(s => s.DocumentIds);

        // Ignore domain events collection (not persisted).
        builder.Ignore(s => s.DomainEvents);
    }
}
