using MediatR;
using Microsoft.AspNetCore.Mvc;
using HealthService.Application.Commands;
using HealthService.Application.Queries;
using HealthService.Application.DTOs;
using HealthService.Presentation.Services;
using HealthService.Presentation.Common;
using FluentValidation;
using HealthService.Application.Queries.GetHealthEventsByAnimal;
using HealthService.Application.Queries.GetHealthEventsByBatch;
using HealthService.Application.Queries.GetHealthEventsByFarm;
using HealthService.Application.Queries.GetHealthEventsByType;

namespace HealthService.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthEventController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly GatewayAuthenticationService _authService;

    public HealthEventController(IMediator mediator, GatewayAuthenticationService authService)
    {
        _mediator = mediator;
        _authService = authService;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<HealthEventResponse>>> Register([FromBody] RegisterHealthEventCommand command)
    {
        var userId = _authService.GetUserId();
        var farmId = _authService.GetFarmId();
        
        // Enforce farm scope if present
        var effectiveFarmId = farmId ?? command.FarmId;
        
        var secureCommand = command with { UserId = userId, FarmId = effectiveFarmId };

        try
        {
            var result = await _mediator.Send(secureCommand);
            return StatusCode(201, ApiResponse<HealthEventResponse>.Ok(result, "Health event registered successfully"));
        }
        catch (ValidationException ex)
        {
            return BadRequest(ApiResponse<HealthEventResponse>.Fail("Validation failed", ex.Errors.Select(e => e.ErrorMessage)));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponse<HealthEventResponse>.Fail(ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<HealthEventResponse>.Fail(ex.Message));
        }
    }

    [HttpGet("farm")]
    public async Task<ActionResult<ApiResponse<HealthEventListResponse>>> GetByFarm(
        [FromQuery] DateOnly? fromDate,
        [FromQuery] DateOnly? toDate,
        [FromQuery] string? eventType)
    {
        var effectiveFarmId = _authService.GetFarmId();

        if (!effectiveFarmId.HasValue || effectiveFarmId.Value <= 0)
            return BadRequest(ApiResponse<HealthEventListResponse>.Fail("User is not associated with a valid Farm (Context Missing)"));

        var query = new GetHealthEventsByFarmQuery(effectiveFarmId.Value, fromDate, toDate, eventType);
        var result = await _mediator.Send(query);
        return Ok(ApiResponse<HealthEventListResponse>.Ok(result));
    }

    [HttpGet("animal/{animalId}")]
    public async Task<ActionResult<ApiResponse<HealthEventListResponse>>> GetByAnimal(
        long animalId,
        [FromQuery] string? eventType)
    {
        // Ideally we should verify if the animal belongs to the user's farm, 
        // but that requires cross-service check or animal duplication. 
        // For now, adhering to strict microservice boundaries without direct DB access to Animal table.
        var result = await _mediator.Send(new GetHealthEventsByAnimalQuery(animalId, eventType));
        return Ok(ApiResponse<HealthEventListResponse>.Ok(result));
    }

    [HttpGet("batch/{batchId}")]
    public async Task<ActionResult<ApiResponse<HealthEventListResponse>>> GetByBatch(
        int batchId,
        [FromQuery] string? eventType)
    {
        // Similar constraint logic applies here
        var result = await _mediator.Send(new GetHealthEventsByBatchQuery(batchId, eventType));
        return Ok(ApiResponse<HealthEventListResponse>.Ok(result));
    }

    [HttpGet("type/{type}")]
    public async Task<ActionResult<ApiResponse<HealthEventListResponse>>> GetByType(
        string type,
        [FromQuery] DateOnly? fromDate,
        [FromQuery] DateOnly? toDate)
    {
        var effectiveFarmId = _authService.GetFarmId();

        if (!effectiveFarmId.HasValue || effectiveFarmId.Value <= 0)
            return BadRequest(ApiResponse<HealthEventListResponse>.Fail("User is not associated with a valid Farm (Context Missing)"));

        var result = await _mediator.Send(new GetHealthEventsByTypeQuery(type, effectiveFarmId.Value, fromDate, toDate));
        return Ok(ApiResponse<HealthEventListResponse>.Ok(result));
    }

    [HttpGet("dashboard-stats")]
    public async Task<ActionResult<ApiResponse<HealthDashboardStats>>> GetDashboardStats()
    {
        var effectiveFarmId = _authService.GetFarmId();

        if (!effectiveFarmId.HasValue || effectiveFarmId.Value <= 0)
            return BadRequest(ApiResponse<HealthDashboardStats>.Fail("User is not associated with a valid Farm (Context Missing)"));

        var result = await _mediator.Send(new GetHealthDashboardStatsQuery(effectiveFarmId.Value));
        return Ok(ApiResponse<HealthDashboardStats>.Ok(result));
    }

    [HttpGet("upcoming")]
    public async Task<ActionResult<ApiResponse<IEnumerable<HealthEventResponse>>>> GetUpcoming([FromQuery] int limit = 10)
    {
        var effectiveFarmId = _authService.GetFarmId();

        if (!effectiveFarmId.HasValue || effectiveFarmId.Value <= 0)
            return BadRequest(ApiResponse<IEnumerable<HealthEventResponse>>.Fail("User is not associated with a valid Farm (Context Missing)"));

        var result = await _mediator.Send(new GetUpcomingHealthEventsQuery(effectiveFarmId.Value, limit));
        return Ok(ApiResponse<IEnumerable<HealthEventResponse>>.Ok(result));
    }

    [HttpGet("recent-treatments")]
    public async Task<ActionResult<ApiResponse<IEnumerable<HealthEventResponse>>>> GetRecentTreatments([FromQuery] int limit = 10)
    {
        var effectiveFarmId = _authService.GetFarmId();

        if (!effectiveFarmId.HasValue || effectiveFarmId.Value <= 0)
            return BadRequest(ApiResponse<IEnumerable<HealthEventResponse>>.Fail("User is not associated with a valid Farm (Context Missing)"));

        var result = await _mediator.Send(new GetRecentTreatmentsQuery(effectiveFarmId.Value, limit));
        return Ok(ApiResponse<IEnumerable<HealthEventResponse>>.Ok(result));
    }
}
