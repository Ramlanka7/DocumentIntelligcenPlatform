# T05 — RAG Pipeline

**Suggested agent:** rag-ai-engineer
**Depends on:** T03, T04
**Spec references:** README.MD §RAG Architecture (314–348), §AI & Search (50–57)

## Objective
Implement the ingestion + retrieval pipeline: chunk → embed → index → retrieve, preserving
citation metadata end-to-end.

## Scope / deliverables
- Chunking strategy (size + overlap, section-aware) producing `DocumentChunk`s with page/section
  refs preserved.
- `IEmbeddingService` impl using Azure OpenAI embeddings.
- `ISearchService` impl using Azure AI Search: index schema, upsert, and **vector + hybrid +
  semantic** retrieval.
- Retrieval returns chunks with scores mapped back to `Citation` (doc/page/paragraph/confidence).
- Config-driven (endpoints/keys/deployment names) via options pattern.

## Out of scope
Answer generation/prompting (T07).

## Acceptance criteria
- Upload→extract→chunk→embed→index runs against the compose stack (or documented local stub).
- A query returns top-k chunks with citations and confidence scores.
- Index schema + chunking params are configurable, not hardcoded.
