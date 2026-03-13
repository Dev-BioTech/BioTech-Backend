using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using HerdService.Application.Commands;
using HerdService.Application.Queries;
using HerdService.Application.DTOs;
using HerdService.Presentation.Services;
using Shared.Infrastructure.Common;

namespace HerdService.Presentation.Controllers.V1;

[ApiController]
[Route("api/v1/paddocks")]
public class PaddocksController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly GatewayAuthenticationService _authService;

    public PaddocksController(IMediator mediator, GatewayAuthenticationService authService)
    {
        _mediator = mediator;
        _authService = authService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<PaddockResponse>>>> GetPaddocksByFarm([FromQuery] int farmId)
    {
        try
        {
            // Validate user has access to this farm
            var userId = _authService.GetUserId();
            if (userId == null)
            {
                return Unauthorized(ApiResponse<IEnumerable<PaddockResponse>>.Fail("User not authenticated"));
            }

            var result = await _mediator.Send(new GetPaddocksByFarmQuery(farmId));
            return Ok(ApiResponse<IEnumerable<PaddockResponse>>.Ok(result, "Paddocks retrieved successfully"));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponse<IEnumerable<PaddockResponse>>.Fail(ex.Message));
        }
        catch (Exception)
        {
            return StatusCode(500, ApiResponse<IEnumerable<PaddockResponse>>.Fail("Internal server error"));
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<PaddockResponse>>> CreatePaddock([FromBody] CreatePaddockCommand command)
    {
        try
        {
            // Validate user has access to this farm
            var userId = _authService.GetUserId();
            if (userId == null)
            {
                return Unauthorized(ApiResponse<PaddockResponse>.Fail("User not authenticated"));
            }

            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetPaddocksByFarm), new { farmId = result.FarmId }, ApiResponse<PaddockResponse>.Ok(result, "Paddock created successfully"));
        }
        catch (ValidationException ex)
        {
            return BadRequest(ApiResponse<PaddockResponse>.Fail("Validation failed", ex.Errors.Select(e => e.ErrorMessage)));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponse<PaddockResponse>.Fail(ex.Message));
        }
        catch (Exception)
        {
            return StatusCode(500, ApiResponse<PaddockResponse>.Fail("Internal server error"));
        }
    }
}
