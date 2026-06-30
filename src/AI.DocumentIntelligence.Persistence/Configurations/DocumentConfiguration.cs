using AI.DocumentIntelligence.Domain.Entities;
using AI.DocumentIntelligence.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AI.DocumentIntelligence.Persistence.Configurations;

internal sealed class DocumentConfiguration : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.ToTable("documents");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id)
            .HasColumnName("id");

        builder.Property(d => d.OwnerId)
            .HasColumnName("owner_id")
            .IsRequired();

        builder.Property(d => d.Type)
            .HasColumnName("type")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(d => d.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(d => d.StoragePath)
            .HasColumnName("storage_path")
            .HasMaxLength(1024)
            .IsRequired();

        builder.Property(d => d.ExtractedText)
            .HasColumnName("extracted_text");

        builder.Property(d => d.FailureReason)
            .HasColumnName("failure_reason")
            .HasMaxLength(2048);

        builder.Property(d => d.CreatedAtUtc)
            .HasColumnName("created_at_utc")
            .IsRequired();

        builder.Property(d => d.UpdatedAtUtc)
            .HasColumnName("updated_at_utc");

        // FileMetadata owned type stored as JSON column.
        builder.OwnsOne(d => d.Metadata, meta =>
        {
            meta.ToJson("metadata");
            meta.Property(m => m.FileName).HasColumnName("file_name").HasMaxLength(512).IsRequired();
            meta.Property(m => m.SizeBytes).HasColumnName("size_bytes").IsRequired();
            meta.Property(m => m.PageCount).HasColumnName("page_count").IsRequired();
            meta.Property(m => m.ContentType).HasColumnName("content_type").HasMaxLength(128).IsRequired();
        });

        builder.HasIndex(d => d.OwnerId)
            .HasDatabaseName("ix_documents_owner_id");

        builder.HasIndex(d => d.Status)
            .HasDatabaseName("ix_documents_status");

        // One-to-many: Document owns its chunks.
        builder.HasMany(d => d.Chunks)
            .WithOne()
            .HasForeignKey(c => c.DocumentId)
            .OnDelete(DeleteBehavior.Cascade);

        // Ignore domain events collection (not persisted).
        builder.Ignore(d => d.DomainEvents);
    }
}
