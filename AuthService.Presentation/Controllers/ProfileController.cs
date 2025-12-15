using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthService.Application.Commands;
using AuthService.Application.DTOs;
using AuthService.Application.Queries;
using System.Security.Claims;
using System;

namespace AuthService.Presentation.Controllers;

[ApiController]
[Route("api/auth/profile")]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProfileController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<UserProfileDto>> GetProfile()
    {
        // Extract userId from JWT claims
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
        {
            return Unauthorized("Invalid Token: User ID not found.");
        }

        try
        {
            var profile = await _mediator.Send(new GetProfileQuery(userId));
            return Ok(profile);
        }
        catch (KeyNotFoundException)
        {
            return NotFound("User not found.");
        }
    }

    [HttpPut]
    public async Task<ActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
    {
        // Extract userId from JWT claims
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
        {
            return Unauthorized("Invalid Token: User ID not found.");
        }

        try
        {
            await _mediator.Send(new UpdateProfileCommand(userId, dto));
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound("User not found.");
        }
    }
}
