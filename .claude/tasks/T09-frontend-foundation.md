# T09 — Frontend Foundation

**Suggested agent:** angular-frontend-engineer
**Depends on:** T00
**Spec references:** README.MD §Frontend (22–31), §Landing Page (124–150),
§Authentication (104–121)

## Objective
Build the Angular 20 app shell, theming, auth plumbing, and the landing page.

## Scope / deliverables
- App shell: routing, layout, navigation; standalone components + signals throughout; OnPush.
- Angular Material + Tailwind + a polished **dark enterprise theme**; responsive; a11y baseline.
- HTTP interceptors: auth token attach, refresh-on-401, error toast; route guards by role.
- Auth UI: login + token/refresh handling; signal-based auth state store.
- Typed API client services scaffolding generated from / aligned to the T08 contracts.
- **Landing page**: hero (title + subtitle from spec), two feature cards (Analysis Mode,
  Comparison Mode) linking to `/analysis` and `/compare`, subtle animations.

## Out of scope
Analysis/comparison/chat feature internals (T10–T12), admin (T13).

## Acceptance criteria
- App builds and runs; landing page matches spec copy and dark styling.
- Auth guard redirects unauthenticated users; 401 triggers refresh.
- No `any`; components standalone + OnPush; passes lint.
