using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using HerdService.Application.Commands;
using HerdService.Application.Queries.GetBatchesByFarm;
using HerdService.Application.DTOs;
using HerdService.Presentation.Services;
using Shared.Infrastructure.Common;

namespace HerdService.Presentation.Controllers.V1;

[ApiController]
[Route("api/v1/batches")]
public class BatchesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly GatewayAuthenticationService _authService;

    public BatchesController(IMediator mediator, GatewayAuthenticationService authService)
    {
        _mediator = mediator;
        _authService = authService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<BatchResponse>>>> GetBatchesByFarm([FromQuery] int farmId)
    {
        try
        {
            // Validate user has access to this farm (Simplified check for now)
            var userId = _authService.GetUserId();
            if (userId == null)
            {
                return Unauthorized(ApiResponse<IEnumerable<BatchResponse>>.Fail("User not authenticated"));
            }

            var result = await _mediator.Send(new GetBatchesByFarmQuery(farmId));
            return Ok(ApiResponse<IEnumerable<BatchResponse>>.Ok(result, "Batches retrieved successfully"));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponse<IEnumerable<BatchResponse>>.Fail(ex.Message));
        }
        catch (Exception)
        {
            return StatusCode(500, ApiResponse<IEnumerable<BatchResponse>>.Fail("Internal server error"));
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<BatchResponse>>> CreateBatch([FromBody] CreateBatchCommand command)
    {
        try
        {
            // Validate user has access to this farm
            var userId = _authService.GetUserId();
            if (userId == null)
            {
                return Unauthorized(ApiResponse<BatchResponse>.Fail("User not authenticated"));
            }

            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetBatchesByFarm), new { farmId = result.FarmId }, ApiResponse<BatchResponse>.Ok(result, "Batch created successfully"));
        }
        catch (ValidationException ex)
        {
            return BadRequest(ApiResponse<BatchResponse>.Fail("Validation failed", ex.Errors.Select(e => e.ErrorMessage)));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponse<BatchResponse>.Fail(ex.Message));
        }
        catch (Exception)
        {
            return StatusCode(500, ApiResponse<BatchResponse>.Fail("Internal server error"));
        }
    }
}
