# T03 — Application Core (CQRS)

**Suggested agent:** dotnet-backend-engineer
**Depends on:** T01, T02
**Spec references:** README.MD §Architecture Requirements (73–100), §AI Service Layer (375–394),
§Document Processing Layer (350–372)

## Objective
Build the CQRS backbone and declare the service abstractions the rest of the system implements.

## Scope / deliverables
- MediatR registration; `ICommand`/`IQuery`/handler base contracts returning `Result<T>`.
- Pipeline **behaviors**: validation (FluentValidation), logging, performance/timing, and
  unhandled-exception-to-Result.
- AutoMapper profiles scaffold; FluentValidation wiring.
- **Interfaces** (consumed by Infrastructure later):
  `IDocumentProcessor`, `IAIProvider`, `IAnalysisService`, `IComparisonService`,
  `IChatService`, `IEmbeddingService`, `ISearchService`, `ICurrentUser`, `IFileStorage`.
- DI extension `AddApplication()`.
- DTOs/contracts for analysis/comparison/chat results (including `Citation`).

## Out of scope
Concrete implementations of the AI/search/processor interfaces.

## Acceptance criteria
- A trivial sample query flows through MediatR + validation behavior and returns `Result<T>`.
- All listed interfaces compile and are documented with XML summaries.
- Application project references only Domain (not Infrastructure/Persistence/Api).
