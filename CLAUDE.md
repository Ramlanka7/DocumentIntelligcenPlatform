# AI Document Intelligence Platform

## Overview
Enterprise platform to upload, analyze, compare, and chat with documents using AI reasoning
models and Retrieval-Augmented Generation (RAG). Full product/architecture spec lives in
[.claude/README.MD](.claude/README.MD) — treat it as the source of truth for requirements.

This repo is built **incrementally** via numbered task briefs in
[.claude/tasks/INDEX.md](.claude/tasks/INDEX.md). Read the relevant task file before working.

## Tech stack & versions
- **Frontend**: Angular 20 — standalone components, Angular Signals, Angular Material,
  Tailwind CSS, TypeScript (strict), responsive, dark theme.
- **Backend**: .NET 10 Web API — Clean Architecture, CQRS via MediatR, FluentValidation,
  AutoMapper, Repository + Unit of Work.
- **Database**: PostgreSQL with the `pgvector` extension.
- **AI & search**: Azure OpenAI (Microsoft Foundry) + Azure AI Search (vector / hybrid /
  semantic). Provider is abstracted (`IAIProvider`); **Azure OpenAI is the default**, with
  Anthropic Claude / OpenAI / Ollama supported behind the same interface.
- **Observability**: Serilog, Application Insights, OpenTelemetry.
- **DevOps**: Docker, Docker Compose, GitHub Actions CI/CD.

## Architecture rules (non-negotiable)
- **Clean Architecture dependency direction**: `Api → Application → Domain`. `Infrastructure`
  and `Persistence` implement interfaces declared in `Application`/`Domain`. **`Domain`
  depends on nothing.** Never reference outer layers from inner ones.
- **CQRS + MediatR**: every use case is a Command or Query with its own handler. No business
  logic in controllers.
- **Result pattern**: return `Result`/`Result<T>` for expected failures. Do **not** throw
  exceptions for control flow; exceptions are for the global exception middleware only.
- **Repository + Unit of Work** for all data access; no `DbContext` use outside Persistence.
- **FluentValidation** for input validation (via a MediatR validation behavior).
- **AutoMapper** for entity↔DTO mapping.
- **API versioning** on all endpoints; **global exception handling middleware** for unhandled
  errors.
- **Dependency Injection** everywhere; depend on interfaces, not concretes.
- Design every abstraction for **extensibility** — new AI providers, document types, and
  workflows must drop in without modifying existing code (Open/Closed).

## Solution layout
```
src/
  AI.DocumentIntelligence.Api            # Web API, controllers, middleware, DI composition
  AI.DocumentIntelligence.Application    # CQRS, handlers, interfaces, validators, mappings
  AI.DocumentIntelligence.Domain         # Entities, value objects, enums, domain errors
  AI.DocumentIntelligence.Infrastructure # AI providers, Azure AI Search, doc processors, auth
  AI.DocumentIntelligence.Persistence    # EF Core DbContext, configs, repositories, migrations
  AI.DocumentIntelligence.Tests          # Unit + integration tests
frontend/                                # Angular 20 workspace
```

## Conventions
- **C#**: nullable enabled, file-scoped namespaces, one public type per file, `Async` suffix on
  async methods, `private readonly` fields, records for DTOs/value objects. Treat warnings as
  errors where practical.
- **Angular**: standalone components, `ChangeDetectionStrategy.OnPush`, signal-based state,
  typed reactive forms, no `any`, feature-folder structure, `kebab-case` filenames.
- **Citations**: every AI analysis/comparison/chat response MUST carry citations —
  document name, page number, paragraph reference, and confidence score.
- **Commits**: small, scoped, one task per branch (`task/T0X-short-name`). Conventional-style
  messages. Do not commit secrets; use config/user-secrets/env.

## Commands
> Filled in once the T00 skeleton exists. Expected:
- Backend: `dotnet build`, `dotnet test`, `dotnet format`
- Frontend: `npm ci && npm run build`, `npm test` (in `frontend/`)
- Local stack: `docker compose up -d`

## Workflow
1. Open [.claude/tasks/INDEX.md](.claude/tasks/INDEX.md), pick the next `Not started` task whose
   dependencies are `Done`.
2. Read that task file in full (it references exact spec line ranges).
3. Delegate to the **Suggested agent** named in the task.
4. Satisfy every acceptance criterion; run `architecture-reviewer` before finishing.
5. Update the task's status in INDEX.md to `Done`.
