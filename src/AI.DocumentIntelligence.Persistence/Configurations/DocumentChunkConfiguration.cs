using AI.DocumentIntelligence.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Pgvector;

namespace AI.DocumentIntelligence.Persistence.Configurations;

internal sealed class DocumentChunkConfiguration : IEntityTypeConfiguration<DocumentChunk>
{
    private const int EmbeddingDimensions = 1536; // Azure OpenAI text-embedding-3-small

    public void Configure(EntityTypeBuilder<DocumentChunk> builder)
    {
        builder.ToTable("document_chunks");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasColumnName("id");

        builder.Property(c => c.DocumentId)
            .HasColumnName("document_id")
            .IsRequired();

        builder.Property(c => c.Index)
            .HasColumnName("index")
            .IsRequired();

        builder.Property(c => c.Content)
            .HasColumnName("content")
            .IsRequired();

        builder.Property(c => c.PageNumber)
            .HasColumnName("page_number")
            .IsRequired();

        builder.Property(c => c.ParagraphReference)
            .HasColumnName("paragraph_reference")
            .HasMaxLength(512)
            .IsRequired();

        builder.Property(c => c.TokenCount)
            .HasColumnName("token_count")
            .IsRequired();

        // Map IReadOnlyList<float>? to the pgvector column type via value converter.
        builder.Property(c => c.Embedding)
            .HasColumnName("embedding")
            .HasColumnType($"vector({EmbeddingDimensions})")
            .HasConversion(
                v => v == null ? null : new Vector(v.ToArray()),
                v => v == null ? null : (IReadOnlyList<float>)v.Memory.ToArray());

        builder.Property(c => c.CreatedAtUtc)
            .HasColumnName("created_at_utc")
            .IsRequired();

        builder.Property(c => c.UpdatedAtUtc)
            .HasColumnName("updated_at_utc");

        builder.HasIndex(c => c.DocumentId)
            .HasDatabaseName("ix_document_chunks_document_id");

        // HNSW cosine similarity index for semantic search.
        builder.HasIndex(c => c.Embedding)
            .HasMethod("hnsw")
            .HasOperators("vector_cosine_ops")
            .HasDatabaseName("ix_document_chunks_embedding_hnsw_cosine");

        // Ignore domain events collection (not persisted).
        builder.Ignore(c => c.DomainEvents);
    }
}
