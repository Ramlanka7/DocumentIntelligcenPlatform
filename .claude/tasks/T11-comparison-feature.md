# T11 — Comparison Feature

**Suggested agent:** angular-frontend-engineer
**Depends on:** T08, T09
**Spec references:** README.MD §Comparison Mode (244–296)

## Objective
Deliver the `/compare` experience with a GitHub-style diff viewer.

## Scope / deliverables
- Upload 2–4 documents (reuse the T10 upload component).
- Comparison type selector: side-by-side, version, contract, policy, custom.
- Results: executive overview, key differences, risk analysis, recommendations, detailed
  change log, source citations.
- **Visual diff viewer** (GitHub-style) highlighting Added / Removed / Modified content and
  clause/pricing/risk/compliance differences.
- Signal-based comparison session state; loading/error states.

## Out of scope
Analysis (T10), export (T14).

## Acceptance criteria
- 2–4 doc comparison runs end-to-end; diff viewer renders added/removed/modified clearly.
- Citations present on differences; change log complete.
- Responsive + accessible; passes lint.
