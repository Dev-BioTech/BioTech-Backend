using FeedingService.Application.Commands;
using FeedingService.Application.DTOs;
using FeedingService.Application.Queries.GetFeedingEventById;
using FeedingService.Application.Queries.GetFeedingEventsByBatch;
using FeedingService.Application.Queries.GetFeedingEventsByFarm;
using FeedingService.Presentation.Common;
using FeedingService.Presentation.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FeedingService.Presentation.Controllers.V1;


[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
[Produces("application/json")]
public class FeedingEventsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<FeedingEventsController> _logger;
    private readonly GatewayAuthenticationService _authService;

    public FeedingEventsController(
        IMediator mediator,
        ILogger<FeedingEventsController> logger,
        GatewayAuthenticationService authService)
    {
        _mediator = mediator;
        _logger = logger;
        _authService = authService;
    }

    /// <summary>
    /// Get feeding event by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<FeedingEventResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id, CancellationToken ct)
    {
        _logger.LogInformation("Getting feeding event with id: {Id}", id);

        var query = new GetFeedingEventByIdQuery(id);
        var result = await _mediator.Send(query, ct);

        if (result == null)
            return NotFound(ApiResponse<FeedingEventResponse>.Fail($"Feeding event with id {id} not found"));

        return Ok(ApiResponse<FeedingEventResponse>.Ok(result));
    }

    /// <summary>
    /// Get feeding events by farm
    /// </summary>
    [HttpGet("farm/{farmId}")]
    [ProducesResponseType(typeof(ApiResponse<FeedingEventListResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByFarm(
        int farmId,
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate,
        CancellationToken ct)
    {
        _logger.LogInformation("Getting feeding events for farm: {FarmId}", farmId);

        var query = new GetFeedingEventsByFarmQuery(farmId, fromDate, toDate);
        var result = await _mediator.Send(query, ct);

        return Ok(ApiResponse<FeedingEventListResponse>.Ok(result));
    }

    /// <summary>
    /// Get feeding events by batch
    /// </summary>
    [HttpGet("batch/{batchId}")]
    [ProducesResponseType(typeof(ApiResponse<FeedingEventListResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByBatch(int batchId, CancellationToken ct)
    {
        _logger.LogInformation("Getting feeding events for batch: {BatchId}", batchId);

        var query = new GetFeedingEventsByBatchQuery(batchId);
        var result = await _mediator.Send(query, ct);

        return Ok(ApiResponse<FeedingEventListResponse>.Ok(result));
    }

    /// <summary>
    /// Create a new feeding event
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FeedingEventResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateFeedingEventRequest request,
        CancellationToken ct)
    {
        _logger.LogInformation("Creating new feeding event for farm: {FarmId}", request.FarmId);

        var userId = _authService.GetUserId();

        var command = new CreateFeedingEventCommand(
            request.FarmId,
            request.SupplyDate,
            request.DietId,
            request.BatchId,
            request.AnimalId,
            request.ProductId,
            request.TotalQuantity,
            request.AnimalsFedCount,
            request.UnitCostAtMoment,
            request.Observations,
            request.RegisteredBy,
            userId
        );

        var result = await _mediator.Send(command, ct);

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Id },
            ApiResponse<FeedingEventResponse>.Ok(result, "Feeding event created successfully"));
    }

    /// <summary>
    /// Update an existing feeding event
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<FeedingEventResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        long id,
        [FromBody] UpdateFeedingEventRequest request,
        CancellationToken ct)
    {
        if (id != request.Id)
            return BadRequest(ApiResponse<FeedingEventResponse>.Fail("ID mismatch"));

        _logger.LogInformation("Updating feeding event: {Id}", id);

        var userId = _authService.GetUserId();

        var command = new UpdateFeedingEventCommand(
            request.Id,
            request.FarmId,
            request.SupplyDate,
            request.DietId,
            request.BatchId,
            request.AnimalId,
            request.ProductId,
            request.TotalQuantity,
            request.AnimalsFedCount,
            request.UnitCostAtMoment,
            request.Observations,
            userId
        );

        var result = await _mediator.Send(command, ct);

        return Ok(ApiResponse<FeedingEventResponse>.Ok(result, "Feeding event updated successfully"));
    }
}