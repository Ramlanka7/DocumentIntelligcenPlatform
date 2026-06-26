# T01 — Domain Layer

**Suggested agent:** dotnet-backend-engineer
**Depends on:** T00
**Spec references:** README.MD §Persistence Requirements (398–423), §Roles (116–121),
§Comparison Types (252–258), §Analysis Capabilities (192–203)

## Objective
Model the core domain in `AI.DocumentIntelligence.Domain` with no external dependencies.

## Scope / deliverables
- **Entities**: `User`, `Document`, `DocumentChunk`, `AnalysisSession`, `ComparisonSession`,
  `ChatSession`, `ChatMessage`, `AuditLog`, `AiUsageMetric`.
- **Value objects / owned types**: `Citation` (document name, page, paragraph, confidence),
  `FileMetadata` (name, size, page count, content type), token/cost figures.
- **Enums**: `UserRole` (Admin/Analyst/Viewer), `DocumentStatus`, `DocumentType`
  (Pdf/Docx/Txt/Csv/Pptx), `AnalysisCapability`, `ComparisonType`, `ChangeStatus`
  (Added/Removed/Modified), `SessionStatus`.
- **Result pattern primitives**: `Result`, `Result<T>`, `Error` (code + message + type), and a
  catalog of domain `Errors`.
- Base entity (`Id`, audit timestamps) and domain event hooks if useful.

## Out of scope
EF configuration, persistence, services.

## Acceptance criteria
- Domain project compiles with **no references** to other src projects or infrastructure.
- All entities/enums/value objects from the spec's persistence + roles sections are present.
- `Result`/`Error` types support the no-exceptions-for-control-flow rule.
