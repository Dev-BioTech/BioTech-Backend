using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using AuthService.Application.Commands;
using AuthService.Application.DTOs;

namespace AuthService.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto)
    {
        try
        {
            var response = await _mediator.Send(new LoginCommand(loginDto));
            return Ok(response);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized("Invalid credentials");
        }
    }

    [HttpPost("register")]
    public async Task<ActionResult<int>> Register([FromBody] RegisterUserDto registerDto)
    {
        var userId = await _mediator.Send(new RegisterUserCommand(registerDto));
        return CreatedAtAction(nameof(Login), null, new { id = userId });
    }
}
