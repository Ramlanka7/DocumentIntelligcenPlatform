# T07 — AI Service Layer

**Suggested agent:** rag-ai-engineer
**Depends on:** T03, T05
**Spec references:** README.MD §AI Service Layer (375–394), §Analysis Capabilities (192–213),
§Comparison (244–296), §AI Chat (298–311), §AI Usage Metrics (414–423)

> **Before wiring providers, consult the `claude-api` skill** for current model IDs/params,
> especially for the Anthropic provider.

## Objective
Implement AI-backed analysis, comparison, and chat behind a configurable provider abstraction.

## Scope / deliverables
- `IAIProvider` concrete: **AzureOpenAiProvider** (default). Stub registrations + factory for
  `OpenAiProvider`, `AnthropicProvider`, `OllamaProvider` — selectable by config (Open/Closed).
- `IAnalysisService`: executive summary, key insights, action items, risk, compliance,
  financial, sentiment, custom Q&A — structured output with citations.
- `IComparisonService`: side-by-side/version/contract/policy/custom; identifies added/removed/
  modified, clause/pricing/risk/compliance differences; produces a change log + citations.
- `IChatService`: RAG-grounded conversational answers with citations.
- Prompt templates centralized; structured (JSON) response parsing into the T03 DTOs.
- **Usage tracking**: prompt/completion tokens, cost, processing time → `AiUsageMetric`.

## Out of scope
Controllers (T08), UI (T10–T13).

## Acceptance criteria
- Each service returns structured results with valid citations.
- Switching provider via config requires no code change in callers.
- Token/cost/time captured per call and persisted.
