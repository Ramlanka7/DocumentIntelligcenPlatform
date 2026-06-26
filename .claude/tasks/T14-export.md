# T14 — Export Features

**Suggested agent:** dotnet-backend-engineer
**Depends on:** T08, T10, T11
**Spec references:** README.MD §Export Features (443–451)

## Objective
Export analysis and comparison results to PDF, Word, Excel, and Markdown.

## Scope / deliverables
- `IExportService` + format implementations (PDF, Word/DOCX, Excel/XLSX, Markdown).
- Export endpoints returning the file with correct content types/filenames.
- Exports include citations and preserve structure (summary, findings, change log, etc.).
- Frontend: export buttons on analysis + comparison results with format choice + download.

## Out of scope
New analysis logic.

## Acceptance criteria
- All four formats download and open correctly with complete, cited content.
- Large results export without timing out (streamed where appropriate).
