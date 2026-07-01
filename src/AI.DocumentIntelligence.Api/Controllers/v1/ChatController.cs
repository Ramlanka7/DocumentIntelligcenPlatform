using AI.DocumentIntelligence.Api.Extensions;
using AI.DocumentIntelligence.Application.Contracts.Chat;
using AI.DocumentIntelligence.Application.Features.Chat;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AI.DocumentIntelligence.Api.Controllers.v1;

/// <summary>RAG-grounded chat: send a question about one or more documents and receive a cited answer.</summary>
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/chat")]
[Authorize(Policy = "ViewerOrAbove")]
[Produces("application/json")]
public sealed class ChatController(ISender sender) : ControllerBase
{
    /// <summary>Asks a question grounded in the specified documents and returns a cited AI answer.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ChatResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AskAsync(
        [FromBody] ChatCommand command,
        CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);
        return result.ToActionResult(this);
    }
}
