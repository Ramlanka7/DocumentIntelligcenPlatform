# T02 — Persistence (EF Core + pgvector)

**Suggested agent:** dotnet-backend-engineer
**Depends on:** T00, T01
**Spec references:** README.MD §Database (45–48), §Persistence Requirements (398–423)

## Objective
Implement data access in `AI.DocumentIntelligence.Persistence` over PostgreSQL + pgvector.

## Scope / deliverables
- `ApplicationDbContext` + `IEntityTypeConfiguration<T>` per aggregate.
- pgvector mapping for `DocumentChunk` embeddings (vector column + index).
- Generic `IRepository<T>` + `IUnitOfWork` interfaces (declared in Application/Domain) with
  EF implementations here.
- Specific repositories where queries warrant (documents, sessions, audit, usage metrics).
- Initial EF Core migration; idempotent DB init that enables the `vector` extension.
- Seed data: roles + a default admin user (configurable, no hardcoded prod secret).
- DI registration extension `AddPersistence(configuration)`.

## Out of scope
Embedding generation (T05), business handlers (T03+).

## Acceptance criteria
- `dotnet ef migrations add` / `database update` succeed against the compose postgres.
- pgvector column + similarity index created; a smoke query runs.
- No `DbContext` usage leaks outside the Persistence project.
