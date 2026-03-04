using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using HerdService.Application.Commands;
using HerdService.Application.Queries;
using HerdService.Application.DTOs;
using HerdService.Presentation.Services;
using HerdService.Presentation.Common;

namespace HerdService.Presentation.Controllers.V1;

[ApiController]
[Route("api/v1/[controller]")]
public class MovementTypesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly GatewayAuthenticationService _authService;

    public MovementTypesController(IMediator mediator, GatewayAuthenticationService authService)
    {
        _mediator = mediator;
        _authService = authService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<MovementTypeResponse>>>> GetAllMovementTypes()
    {
        try
        {
            var result = await _mediator.Send(new GetAllMovementTypesQuery());
            return Ok(ApiResponse<IEnumerable<MovementTypeResponse>>.Ok(result, "Movement types retrieved successfully"));
        }
        catch (Exception)
        {
            return StatusCode(500, ApiResponse<IEnumerable<MovementTypeResponse>>.Fail("Internal server error"));
        }
    }
}
