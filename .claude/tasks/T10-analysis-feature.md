# T10 — Analysis Feature

**Suggested agent:** angular-frontend-engineer
**Depends on:** T08, T09
**Spec references:** README.MD §Analysis Mode (152–190), §Analysis Capabilities (192–213),
§Analysis Results (216–242)

## Objective
Deliver the `/analysis` experience end-to-end against the API.

## Scope / deliverables
- Upload UI: drag & drop, browse, existing-file selection, remove, **upload progress**;
  client validation mirroring server rules (≤4 docs, formats, combined pages/size).
- File list display: name, size, page count, upload status.
- Analysis options UI for all capabilities (summary, insights, action items, risk, compliance,
  financial, sentiment, custom Q&A) + example query chips.
- Results view: executive summary, key findings, risks, recommendations, action items, and
  **referenced sources** with document/page/paragraph/confidence citations.
- Loading/empty/error states; signal-based session state.

## Out of scope
Comparison (T11), chat (T12), export (T14).

## Acceptance criteria
- Upload→analyze→view works against the running API with real citations rendered.
- Client validation matches server; oversize/too-many handled gracefully.
- Responsive + accessible; passes lint.
