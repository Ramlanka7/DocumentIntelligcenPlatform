# T16 — Testing

**Suggested agent:** test-engineer
**Depends on:** T08–T14
**Spec references:** README.MD §Testing Requirements (454–477), §Non-Functional (481–496)

## Objective
Comprehensive automated tests across backend and frontend.

## Scope / deliverables
- **Unit tests**: handlers, services (analysis/comparison/chat/AI provider with mocks),
  validators, repositories.
- **Integration tests**: API endpoints, authentication/refresh, database operations using
  **Testcontainers** (postgres + pgvector); AI/search providers faked.
- **Frontend tests**: upload flow, analysis flow, comparison flow (+ chat if feasible).
- Coverage reporting; test data fixtures for each document format.

## Out of scope
Load/perf testing.

## Acceptance criteria
- `dotnet test` and frontend tests **all pass**.
- Integration tests run against ephemeral Testcontainers DB.
- Critical paths (auth, upload validation, analysis, comparison) covered.
