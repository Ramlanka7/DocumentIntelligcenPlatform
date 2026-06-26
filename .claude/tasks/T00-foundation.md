# T00 — Foundation & Repo Setup

**Suggested agent:** devops-engineer
**Depends on:** —
**Spec references:** README.MD §Technology Stack (20–70), §Project Structure (86–100), §DevOps (65–69)

## Objective
Stand up an empty but compiling solution and a runnable local stack, so every later task starts
from a green baseline.

## Scope / deliverables
- `git init`, `.gitignore` (dotnet + node + IDE), `.editorconfig`, `Directory.Build.props`
  (nullable enable, treat-warnings, langversion), `README.md` at root.
- .NET solution `AI.DocumentIntelligence.sln` + the 6 projects from the spec under `src/`,
  with correct project references enforcing Clean Architecture direction.
- Angular 20 workspace under `frontend/` (standalone, routing, Material, Tailwind configured),
  no features yet beyond a placeholder shell.
- `Dockerfile` for the API and `frontend/Dockerfile`; `docker-compose.yml` wiring: api,
  `postgres` with `pgvector`, and an Azure AI Search stub/placeholder + Azurite for blobs.
- Central package management (`Directory.Packages.props`) if practical.
- Fill the **Commands** section of root `CLAUDE.md` with the real build/test/run commands.

## Out of scope
Any domain logic, entities, endpoints, or UI features.

## Acceptance criteria
- `dotnet build` succeeds with zero warnings/errors.
- `npm ci && npm run build` in `frontend/` succeeds.
- `docker compose up -d` brings up postgres (pgvector available) and the API health endpoint.
- Project reference graph matches Clean Architecture (Domain references nothing).
