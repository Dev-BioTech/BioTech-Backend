using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AIService.Application.Queries.GetChatResponse;
using AIService.Application.DTOs;
using AIService.Presentation.Services;

namespace AIService.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ChatController : ControllerBase
{
    private readonly ISender _sender;
    private readonly GatewayAuthenticationService _authService;

    public ChatController(ISender sender, GatewayAuthenticationService authService)
    {
        _sender = sender;
        _authService = authService;
    }

    [HttpPost]
    public async Task<IActionResult> Chat([FromBody] ChatRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Message))
            return BadRequest("Message cannot be empty.");

        var userId = _authService.GetUserId();
        var farmId = _authService.GetFarmId();

        if (!userId.HasValue || userId.Value <= 0)
            return Unauthorized("User ID not found in token");

        if (!farmId.HasValue || farmId.Value <= 0)
            return BadRequest("User is not associated with a valid Farm");

        try 
        {
            var response = await _sender.Send(new GetChatResponseQuery(request.Message, farmId.Value, userId.Value));
            return Ok(new { response });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}
