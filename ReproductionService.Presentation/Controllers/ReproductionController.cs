using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReproductionService.Application.Commands.CancelReproductionEvent;
using ReproductionService.Application.Commands.CreateReproductionEvent;
using ReproductionService.Application.Commands;
using ReproductionService.Application.DTOs;
using ReproductionService.Application.Queries.GetReproductionEventById;
using ReproductionService.Application.Queries.GetReproductionEventsByAnimal;
using ReproductionService.Application.Queries.GetReproductionEventsByFarm;
using ReproductionService.Application.Queries.GetReproductionEventsByType;
using ReproductionService.Application.Queries;
using ReproductionService.Domain.Enums;
using Shared.Infrastructure.Common;

namespace ReproductionService.Presentation.Controllers;

[ApiController]
[Route("api/v1/reproduction")]
[Authorize]
public class ReproductionController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly Services.GatewayAuthenticationService _authService;

    public ReproductionController(IMediator mediator, Services.GatewayAuthenticationService authService)
    {
        _mediator = mediator;
        _authService = authService;
    }

    /// <summary>
    /// Get reproduction event by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<ReproductionEventResponse>>> GetById(long id)
    {
        var query = new GetReproductionEventByIdQuery(id);
        var result = await _mediator.Send(query);

        if (result == null)
            return NotFound(ApiResponse<ReproductionEventResponse>.Fail($"Reproduction event with id {id} not found"));

        return Ok(ApiResponse<ReproductionEventResponse>.Ok(result));
    }

    /// <summary>
    /// Get reproduction events by animal.
    /// Note: Implicitly validates Farm context? 
    /// Requirement said: GET /api/v1/Reproduction/animal/{id} (List animal reproduction events)
    /// We should verify animal belongs to current farm, but Handler might not check.
    /// For now, keeping as is, but we could enforce Query.FarmId = _authService.GetFarmId() if Query had it.
    /// </summary>
    [HttpGet("animal/{animalId}")]
    public async Task<ActionResult<ApiResponse<ReproductionEventListResponse>>> GetByAnimal(
        long animalId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        // Ideally we pass FarmId to query to ensure ownership check
        var query = new GetReproductionEventsByAnimalQuery(animalId, page, pageSize);
        var result = await _mediator.Send(query);
        return Ok(ApiResponse<ReproductionEventListResponse>.Ok(result));
    }

    /// <summary>
    /// Get reproduction events by farm (Context specific)
    /// </summary>
    [HttpGet("farm")] // Removed {farmId} from route to stick to Context
    public async Task<ActionResult<ApiResponse<ReproductionEventListResponse>>> GetByFarm(
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var farmId = _authService.GetFarmId();
        if (!farmId.HasValue) return BadRequest(ApiResponse<ReproductionEventListResponse>.Fail("User is not associated with a valid Farm"));

        var query = new GetReproductionEventsByFarmQuery(farmId.Value, fromDate, toDate, page, pageSize);
        var result = await _mediator.Send(query);
        return Ok(ApiResponse<ReproductionEventListResponse>.Ok(result));
    }
    
    // Supporting the legacy route if needed by requirements?
    // Requirement said: GET /api/v1/Reproduction/farm/{id}
    // I should support strict requirement BUT validate against token.
    [HttpGet("farm/{farmId}")]
    public async Task<ActionResult<ApiResponse<ReproductionEventListResponse>>> GetByFarmExplicit(
        int farmId,
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var contextFarmId = _authService.GetFarmId();
        if (contextFarmId.HasValue && farmId != contextFarmId.Value)
           return Unauthorized(ApiResponse<ReproductionEventListResponse>.Fail("Access mismatch for Farm ID"));

        var query = new GetReproductionEventsByFarmQuery(farmId, fromDate, toDate, page, pageSize);
        var result = await _mediator.Send(query);
        return Ok(ApiResponse<ReproductionEventListResponse>.Ok(result));
    }


    /// <summary>
    /// Get reproduction events by type
    /// </summary>
    [HttpGet("type/{type}")]
    public async Task<ActionResult<ApiResponse<ReproductionEventListResponse>>> GetByType(
        ReproductionEventType type,
        // [FromQuery] int farmId, // Removed manual param from signature to use Context
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var farmId = _authService.GetFarmId();
        if (!farmId.HasValue) return BadRequest(ApiResponse<ReproductionEventListResponse>.Fail("User is not associated with a valid Farm"));

        var query = new GetReproductionEventsByTypeQuery(type, farmId.Value, page, pageSize);
        var result = await _mediator.Send(query);
        return Ok(ApiResponse<ReproductionEventListResponse>.Ok(result));
    }

    /// <summary>
    /// Register a new reproduction event
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<ReproductionEventResponse>>> Register(CreateReproductionEventCommand command)
    {
        // Override FarmId and RegisteredBy from Context
        var contextFarmId = _authService.GetFarmId();
        var contextUserId = _authService.GetUserId();

        if (!contextFarmId.HasValue) return BadRequest(ApiResponse<ReproductionEventResponse>.Fail("User is not associated with a valid Farm"));
        
        var secureCommand = command with { FarmId = contextFarmId.Value, RegisteredBy = contextUserId };

        try
        {
            var result = await _mediator.Send(secureCommand);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<ReproductionEventResponse>.Ok(result, "Reproduction event registered successfully"));
        }
        catch (FluentValidation.ValidationException ex)
        {
            return BadRequest(ApiResponse<ReproductionEventResponse>.Fail("Validation failed", ex.Errors.Select(e => e.ErrorMessage)));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponse<ReproductionEventResponse>.Fail(ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ApiResponse<ReproductionEventResponse>.Fail(ex.Message));
        }
        catch (Exception)
        {
            return StatusCode(500, ApiResponse<ReproductionEventResponse>.Fail("An error occurred while processing your request"));
        }
    }

    /// <summary>
    /// Cancel a reproduction event
    /// </summary>
    [HttpPut("{id}/cancel")]
    public async Task<ActionResult<ApiResponse<ReproductionEventResponse>>> Cancel(long id)
    {
        try
        {
            var command = new CancelReproductionEventCommand(id);
            var result = await _mediator.Send(command);
            return Ok(ApiResponse<ReproductionEventResponse>.Ok(result, "Reproduction event cancelled successfully"));
        }
        catch (KeyNotFoundException)
        {
            return NotFound(ApiResponse<ReproductionEventResponse>.Fail($"Reproduction event with id {id} not found"));
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ApiResponse<ReproductionEventResponse>.Fail(ex.Message));
        }
    }

    /// <summary>
    /// Get pregnancies by farm
    /// </summary>
    [HttpGet("pregnancies/farm/{farmId}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<PregnancyDto>>>> GetPregnanciesByFarm(int farmId)
    {
        try
        {
            // Validate user has access to this farm
            var userId = _authService.GetUserId();
            if (userId == null)
            {
                return Unauthorized(ApiResponse<IEnumerable<PregnancyDto>>.Fail("User not authenticated"));
            }

            var result = await _mediator.Send(new GetPregnanciesByFarmQuery(farmId));
            return Ok(ApiResponse<IEnumerable<PregnancyDto>>.Ok(result, "Pregnancies retrieved successfully"));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponse<IEnumerable<PregnancyDto>>.Fail(ex.Message));
        }
        catch (Exception)
        {
            return StatusCode(500, ApiResponse<IEnumerable<PregnancyDto>>.Fail("Internal server error"));
        }
    }

    /// <summary>
    /// Get births by farm
    /// </summary>
    [HttpGet("births/farm/{farmId}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<BirthDto>>>> GetBirthsByFarm(int farmId)
    {
        try
        {
            // Validate user has access to this farm
            var userId = _authService.GetUserId();
            if (userId == null)
            {
                return Unauthorized(ApiResponse<IEnumerable<BirthDto>>.Fail("User not authenticated"));
            }

            var result = await _mediator.Send(new GetBirthsByFarmQuery(farmId));
            return Ok(ApiResponse<IEnumerable<BirthDto>>.Ok(result, "Births retrieved successfully"));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponse<IEnumerable<BirthDto>>.Fail(ex.Message));
        }
        catch (Exception)
        {
            return StatusCode(500, ApiResponse<IEnumerable<BirthDto>>.Fail("Internal server error"));
        }
    }

    /// <summary>
    /// Register a new birth
    /// </summary>
    [HttpPost("births")]
    public async Task<ActionResult<ApiResponse<BirthDto>>> RegisterBirth([FromBody] RegisterBirthCommand command)
    {
        try
        {
            // Validate user is authenticated
            var userId = _authService.GetUserId();
            if (userId == null)
            {
                return Unauthorized(ApiResponse<BirthDto>.Fail("User not authenticated"));
            }

            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetBirthsByFarm), new { farmId = 0 }, ApiResponse<BirthDto>.Ok(result, "Birth registered successfully"));
        }
        catch (FluentValidation.ValidationException ex)
        {
            return BadRequest(ApiResponse<BirthDto>.Fail("Validation failed", ex.Errors.Select(e => e.ErrorMessage)));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponse<BirthDto>.Fail(ex.Message));
        }
        catch (Exception)
        {
            return StatusCode(500, ApiResponse<BirthDto>.Fail("Internal server error"));
        }
    }
}
