# T12 — Chat Feature

**Suggested agent:** angular-frontend-engineer
**Depends on:** T08, T09
**Spec references:** README.MD §AI Chat With Documents (298–311)

## Objective
Conversational RAG chat over processed documents.

## Scope / deliverables
- Chat UI bound to a processed analysis/comparison session; message history (signals).
- Streaming or progressive responses if the API supports it; otherwise loading states.
- Every assistant message renders **source citations** (doc/page/paragraph/confidence).
- Example prompt suggestions from the spec; multi-document context awareness.
- Persist chat sessions/messages via the API.

## Out of scope
Analysis/comparison generation (T07/T10/T11).

## Acceptance criteria
- User can ask questions about uploaded docs and get RAG-grounded, cited answers.
- Chat history persists and reloads per session.
- Responsive + accessible; passes lint.
