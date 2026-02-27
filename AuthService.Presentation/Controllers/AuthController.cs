using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using AuthService.Application.Commands;
using AuthService.Application.DTOs;
using Shared.Infrastructure.Common;

namespace AuthService.Presentation.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Login([FromBody] LoginDto loginDto)
    {
        try
        {
            var response = await _mediator.Send(new LoginCommand(loginDto));
            return Ok(ApiResponse<AuthResponseDto>.Ok(response, "Login successful"));
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(ApiResponse<AuthResponseDto>.Fail("Invalid credentials"));
        }
    }

    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<int>>> Register([FromBody] RegisterUserDto registerDto)
    {
        try
        {
            var userId = await _mediator.Send(new RegisterUserCommand(registerDto));
            return CreatedAtAction(nameof(Login), null, ApiResponse<int>.Ok(userId, "User registered successfully"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<int>.Fail(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<int>.Fail("An error occurred during registration", new[] { ex.Message }));
        }
    }
}
