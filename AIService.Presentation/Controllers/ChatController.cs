using MediatR;
using Microsoft.AspNetCore.Mvc;
using AIService.Application.Queries.GetChatResponse;
using AIService.Application.DTOs;

namespace AIService.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly ISender _sender;

    public ChatController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> Chat([FromBody] ChatRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Message))
            return BadRequest("Message cannot be empty.");

        try 
        {
            var response = await _sender.Send(new GetChatResponseQuery(request.Message));
            return Ok(new { response });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}
