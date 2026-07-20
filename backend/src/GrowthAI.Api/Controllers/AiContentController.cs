using GrowthAI.Application.Ai;
using Microsoft.AspNetCore.Mvc;

namespace GrowthAI.Api.Controllers;

[ApiController]
[Route("api/ai/content")]
public sealed class AiContentController(IAiContentService aiContentService) : ControllerBase
{
    [HttpPost("generate")]
    public IActionResult Generate([FromBody] GenerateContentRequest request)
    {
        return Ok(aiContentService.Generate(request));
    }
}
