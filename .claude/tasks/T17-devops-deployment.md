# T17 — DevOps & Deployment

**Suggested agent:** devops-engineer
**Depends on:** T00, T08, T09
**Spec references:** README.MD §DevOps (65–69), §Final Deliverables (516–535),
§Non-Functional (481–496)

## Objective
Finalize containerization, CI/CD, and production deployment documentation.

## Scope / deliverables
- Production-grade multi-stage Dockerfiles (API + frontend via nginx); finalized
  `docker-compose.yml` (+ override for prod) including postgres/pgvector.
- GitHub Actions CI: restore/build/test (backend + frontend), lint, docker build, coverage
  gate; CD pipeline (build + push images, deploy step placeholder).
- Configuration/secrets strategy (env, user-secrets, key vault placeholders) — no secrets in repo.
- EF Core migration step in deployment.
- `docs/DEPLOYMENT.md`: prerequisites, env vars, Azure OpenAI/AI Search setup, run + deploy,
  scaling/backup notes.

## Out of scope
New application features.

## Acceptance criteria
- CI passes on a clean checkout (build + all tests green).
- `docker compose up` runs the full stack from built images.
- Deployment docs let a new engineer deploy unaided.
