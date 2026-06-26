# T15 — Observability

**Suggested agent:** dotnet-backend-engineer
**Depends on:** T08
**Spec references:** README.MD §Monitoring & Logging (59–63)

## Objective
Structured logging, tracing, and metrics across the backend.

## Scope / deliverables
- Serilog: structured logs, correlation/request IDs, sinks (console + configurable).
- Application Insights integration (configurable; no-op without connection string).
- OpenTelemetry: traces + metrics for HTTP, EF Core, AI/search calls; export to OTLP.
- Health checks (DB, search, AI provider) exposed at `/health`.
- Dashboards/queries documentation for key metrics (latency, AI cost, error rate).

## Out of scope
Frontend telemetry.

## Acceptance criteria
- Requests produce correlated structured logs + traces locally.
- Health checks report component status.
- Telemetry features degrade gracefully when AppInsights/OTLP not configured.
