namespace AI.DocumentIntelligence.Domain.ValueObjects;

/// <summary>
/// A traceable reference back to a source document supporting an AI statement. Every AI
/// analysis, comparison, and chat response MUST carry one or more citations (platform hard rule).
/// </summary>
/// <param name="DocumentName">The name of the source document.</param>
/// <param name="PageNumber">The 1-based page the referenced content appears on.</param>
/// <param name="ParagraphReference">A locator for the paragraph/section within the page (e.g. <c>"¶3"</c> or a heading).</param>
/// <param name="ConfidenceScore">Model confidence in the citation, from 0.0 to 1.0.</param>
public sealed record Citation(
    string DocumentName,
    int PageNumber,
    string ParagraphReference,
    double ConfidenceScore);
