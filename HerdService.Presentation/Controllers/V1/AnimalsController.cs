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
[Route("api/v1/animals")]
public class AnimalsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly GatewayAuthenticationService _authService;

    public AnimalsController(IMediator mediator, GatewayAuthenticationService authService)
    {
        _mediator = mediator;
        _authService = authService;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<AnimalResponse>>> RegisterAnimal([FromBody] RegisterAnimalCommand command)
    {
        var userId = _authService.GetUserId();
        var farmId = _authService.GetFarmId();

        // If user is scoped to a farm, enforce it
        var effectiveFarmId = farmId ?? command.FarmId;

        var secureCommand = command with { UserId = userId, FarmId = effectiveFarmId };

        try
        {
            var result = await _mediator.Send(secureCommand);
            return Ok(ApiResponse<AnimalResponse>.Ok(result, "Animal registered successfully"));
        }
        catch (ValidationException ex)
        {
            return BadRequest(ApiResponse<AnimalResponse>.Fail("Validation failed", ex.Errors.Select(e => e.ErrorMessage)));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponse<AnimalResponse>.Fail(ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ApiResponse<AnimalResponse>.Fail(ex.Message));
        }
        catch (Exception)
        {
            return StatusCode(500, ApiResponse<AnimalResponse>.Fail("An error occurred while processing your request"));
        }
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<AnimalResponse>>>> GetAnimals([FromQuery] int farmId, [FromQuery] string? status, [FromQuery] bool includeInactive = false)
    {
        var authFarmId = _authService.GetFarmId();
        var effectiveFarmId = authFarmId ?? farmId;

        if (effectiveFarmId <= 0)
            return BadRequest(ApiResponse<IEnumerable<AnimalResponse>>.Fail("Farm ID is required"));

        var result = await _mediator.Send(new GetAnimalsByFarmQuery(effectiveFarmId, status, includeInactive));
        return Ok(ApiResponse<IEnumerable<AnimalResponse>>.Ok(result));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<AnimalResponse>>> GetById(long id)
    {
        var result = await _mediator.Send(new GetAnimalByIdQuery(id));
        if (result == null)
            return NotFound(ApiResponse<AnimalResponse>.Fail("Animal not found"));

        return Ok(ApiResponse<AnimalResponse>.Ok(result));
    }

    // Removed /farm/{farmId} as it is replaced by root Get with query param

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<AnimalResponse>>> UpdateAnimal(long id, [FromBody] UpdateAnimalCommand command)
    {
        if (id != command.Id) return BadRequest(ApiResponse<AnimalResponse>.Fail("ID mismatch"));

        var userId = _authService.GetUserId();
        var secureCommand = command with { UserId = userId };

        try
        {
            var result = await _mediator.Send(secureCommand);
            return Ok(ApiResponse<AnimalResponse>.Ok(result, "Animal updated successfully"));
        }
        catch (KeyNotFoundException) { return NotFound(ApiResponse<AnimalResponse>.Fail("Animal not found")); }
        catch (ArgumentException ex) { return BadRequest(ApiResponse<AnimalResponse>.Fail(ex.Message)); }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<Unit>>> DeleteAnimal(long id)
    {
        try
        {
            await _mediator.Send(new DeleteAnimalCommand(id));
            return Ok(ApiResponse<Unit>.Ok(Unit.Value, "Animal deleted successfully"));
        }
        catch (KeyNotFoundException) { return NotFound(ApiResponse<Unit>.Fail("Animal not found")); }
    }

    [HttpPost("{id}/movements")]
    public async Task<ActionResult<ApiResponse<AnimalResponse>>> RegisterMovement(long id, [FromBody] RegisterAnimalMovementCommand command)
    {
        if (id != command.AnimalId) return BadRequest(ApiResponse<AnimalResponse>.Fail("ID mismatch"));

        var userId = _authService.GetUserId();
        var secureCommand = command with { UserId = userId };

        try
        {
            var result = await _mediator.Send(secureCommand);
            return Ok(ApiResponse<AnimalResponse>.Ok(result, "Movement registered successfully"));
        }
        catch (KeyNotFoundException) { return NotFound(ApiResponse<AnimalResponse>.Fail("Animal not found")); }
        catch (ArgumentException ex) { return BadRequest(ApiResponse<AnimalResponse>.Fail(ex.Message)); }
    }

    [HttpPut("{id}/weight")]
    public async Task<ActionResult<ApiResponse<AnimalResponse>>> UpdateWeight(long id, [FromBody] UpdateAnimalWeightCommand command)
    {
        if (id != command.AnimalId) return BadRequest(ApiResponse<AnimalResponse>.Fail("ID mismatch"));

        var userId = _authService.GetUserId();
        var secureCommand = command with { UserId = userId };

        try
        {
            var result = await _mediator.Send(secureCommand);
            return Ok(ApiResponse<AnimalResponse>.Ok(result, "Weight updated successfully"));
        }
        catch (KeyNotFoundException) { return NotFound(ApiResponse<AnimalResponse>.Fail("Animal not found")); }
        catch (ArgumentException ex) { return BadRequest(ApiResponse<AnimalResponse>.Fail(ex.Message)); }
    }

    [HttpPut("{id}/batch")]
    public async Task<ActionResult<ApiResponse<AnimalResponse>>> MoveToBatch(long id, [FromBody] MoveAnimalToBatchCommand command)
    {
        if (id != command.AnimalId) return BadRequest(ApiResponse<AnimalResponse>.Fail("ID mismatch"));

        var userId = _authService.GetUserId();
        var secureCommand = command with { UserId = userId };

        try
        {
            var result = await _mediator.Send(secureCommand);
            return Ok(ApiResponse<AnimalResponse>.Ok(result, "Animal moved to batch successfully"));
        }
        catch (KeyNotFoundException) { return NotFound(ApiResponse<AnimalResponse>.Fail("Animal or Batch not found")); }
        catch (InvalidOperationException ex) { return BadRequest(ApiResponse<AnimalResponse>.Fail(ex.Message)); }
    }

    [HttpPut("{id}/sell")]
    public async Task<ActionResult<ApiResponse<AnimalResponse>>> SellAnimal(long id, [FromBody] SellAnimalCommand command)
    {
        if (id != command.AnimalId) return BadRequest(ApiResponse<AnimalResponse>.Fail("ID mismatch"));

        var userId = _authService.GetUserId();
        var secureCommand = command with { UserId = userId };

        try
        {
            var result = await _mediator.Send(secureCommand);
            return Ok(ApiResponse<AnimalResponse>.Ok(result, "Animal sold successfully"));
        }
        catch (KeyNotFoundException) { return NotFound(ApiResponse<AnimalResponse>.Fail("Animal not found")); }
        catch (ArgumentException ex) { return BadRequest(ApiResponse<AnimalResponse>.Fail(ex.Message)); }
    }

    [HttpPut("{id}/dead")]
    public async Task<ActionResult<ApiResponse<AnimalResponse>>> MarkAsDead(long id, [FromBody] MarkAnimalAsDeadCommand command)
    {
        if (id != command.AnimalId) return BadRequest(ApiResponse<AnimalResponse>.Fail("ID mismatch"));

        var userId = _authService.GetUserId();
        var secureCommand = command with { UserId = userId };

        try
        {
            var result = await _mediator.Send(secureCommand);
            return Ok(ApiResponse<AnimalResponse>.Ok(result, "Animal marked as dead"));
        }
        catch (KeyNotFoundException) { return NotFound(ApiResponse<AnimalResponse>.Fail("Animal not found")); }
        catch (ArgumentException ex) { return BadRequest(ApiResponse<AnimalResponse>.Fail(ex.Message)); }
    }
}
