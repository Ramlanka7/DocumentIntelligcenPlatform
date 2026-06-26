# T04 — Document Processing Layer

**Suggested agent:** rag-ai-engineer
**Depends on:** T03
**Spec references:** README.MD §Document Processing Layer (350–372), §Supported Formats (160–166)

## Objective
Extract structured content from uploaded documents behind `IDocumentProcessor`.

## Scope / deliverables
- `IDocumentProcessor` resolution by document type (factory/strategy — Open/Closed).
- Implementations: `PdfDocumentProcessor`, `WordDocumentProcessor`, `TextDocumentProcessor`,
  `CsvDocumentProcessor`, `PowerPointDocumentProcessor`.
- Each extracts: text, metadata, **page numbers**, tables, headings, sections — enough to
  build accurate citations downstream.
- Robust handling of corrupt/oversized files returning `Result` errors (no throws).
- DI registration in Infrastructure.

## Out of scope
Chunking/embeddings (T05), AI calls (T07).

## Acceptance criteria
- Each format parses a sample fixture and yields text + page numbers + section structure.
- Adding a new processor requires no change to existing processor code.
- Unit-testable: processors take streams, not file paths tied to disk.
