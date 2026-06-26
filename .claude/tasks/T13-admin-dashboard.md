# T13 — Admin Dashboard

**Suggested agent:** angular-frontend-engineer
**Depends on:** T08, T09
**Spec references:** README.MD §Admin Dashboard (426–439), §AI Usage Metrics (414–423)

## Objective
Admin-only dashboard surfacing platform and AI-usage metrics.

## Scope / deliverables
- Backend: Admin queries/handlers aggregating users, documents, analyses, comparisons, AI
  usage, AI cost, average processing time, recent activity (if not already in T08, add here).
- Frontend: Admin route guarded to `Admin` role; metric cards, **charts**, filtering
  (date range / user / type), recent activity feed.
- Signal-based state; loading/empty/error handling.

## Out of scope
Non-admin features.

## Acceptance criteria
- Dashboard shows all spec metrics with working charts + filters.
- Route accessible only to Admin; others get 403/redirect.
- Responsive + accessible; passes lint.
