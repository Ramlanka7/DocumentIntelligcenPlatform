# T08 — API Layer

**Suggested agent:** dotnet-backend-engineer
**Depends on:** T03, T06, T07
**Spec references:** README.MD §Architecture Requirements (73–84), §Final Deliverables (516–535),
§Analysis/Comparison/Chat routes

## Objective
Expose the platform over a versioned, documented REST API in `AI.DocumentIntelligence.Api`.

## Scope / deliverables
- Controllers: Auth, Documents (upload/list/select/remove), Analysis, Comparison, Chat, Admin.
- Thin controllers → MediatR; **Result→HTTP** mapping helper (200/201/400/401/403/404/409/422).
- API versioning (`/api/v1`), OpenAPI/Swagger with auth.
- Global exception handling middleware (problem-details responses).
- CORS for the Angular app; request size limits aligned with upload rules.
- DI composition root wiring all `Add*` extensions.

## Out of scope
Frontend, export (T14), dashboards data viz (T13 FE).

## Acceptance criteria
- Swagger lists all endpoints; auth flow usable from Swagger.
- End-to-end via API: upload → analyze → chat returns cited results.
- Unhandled errors return consistent problem-details; expected failures return mapped codes.
