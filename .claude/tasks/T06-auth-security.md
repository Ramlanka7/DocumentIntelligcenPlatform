# T06 — Auth & Security

**Suggested agent:** dotnet-backend-engineer
**Depends on:** T02, T03
**Spec references:** README.MD §Authentication & Security (104–121), §Constraints (168–172),
§Secure File Uploads (110–113)

## Objective
Provide authentication, authorization, and upload safety across the API.

## Scope / deliverables
- JWT authentication + **refresh tokens** (rotation + revocation store).
- Role-based authorization (Admin / Analyst / Viewer) with policies.
- `ICurrentUser` implementation from the auth context.
- Secure file uploads: file-type validation (real content sniffing, not just extension),
  per-file + combined size limits, **max 4 documents**, **max 500 combined pages**.
- Rate limiting (per user/endpoint).
- Audit logging middleware writing `AuditLog` entries for sensitive actions.
- Login / refresh / logout commands + handlers.

## Out of scope
UI auth screens (T09).

## Acceptance criteria
- Protected endpoints reject missing/invalid/expired tokens; refresh flow works.
- Role policies enforced (Viewer cannot perform Admin actions).
- Upload validation rejects bad type/oversize/too-many/too-many-pages with `Result` errors.
- Audit entries recorded for auth + document actions.
