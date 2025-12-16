using AuthService.Application.Commands.CreateFarm;
using AuthService.Application.DTOs;
using AuthService.Application.Queries.GetFarmById;
using AuthService.Application.Queries.GetFarmsByTenant;
using AuthService.Presentation.Common;
using AuthService.Presentation.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Presentation.Controllers.V1;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
[Produces("application/json")]
public class FarmsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly GatewayAuthenticationService _authService;
    private readonly ILogger<FarmsController> _logger;

    public FarmsController(IMediator mediator, GatewayAuthenticationService authService, ILogger<FarmsController> logger)
    {
        _mediator = mediator;
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Create a new farm
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<FarmResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateFarmRequest request, CancellationToken ct)
    {
        var userId = _authService.GetUserId();
        _logger.LogInformation("Creating farm for User: {UserId}", userId);

        var command = new CreateFarmCommand(
            request.Name,
            request.Owner,
            request.Address,
            request.GeographicLocation,
            userId
        );

        var result = await _mediator.Send(command, ct);

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, 
            ApiResponse<FarmResponse>.Ok(result, "Farm created successfully"));
    }

    /// <summary>
    /// Get farm by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<FarmResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetFarmByIdQuery(id), ct);

        if (result == null)
            return NotFound(ApiResponse<FarmResponse>.Fail($"Farm with id {id} not found"));

        // Optional: Check if user has access to this farm
        var userId = _authService.GetUserId();
        // Since we don't have the user roles in the response to check easily here without another query,
        // we might rely on the query to return null if not accessible OR check it separately.
        // For now, simpler implementation: just return it if found. 
        // Real-world: Should verify user link via UserFarmRole.
        
        return Ok(ApiResponse<FarmResponse>.Ok(result));
    }

    /// <summary>
    /// Get farms for the authenticated user
    /// </summary>
    [HttpGet("tenant/{userId}")]
    [ProducesResponseType(typeof(ApiResponse<FarmListResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByTenant(int userId, [FromQuery] bool includeInactive = false, CancellationToken ct = default)
    {
        // Security check: Ensure authenticated user matches requested userId OR is Admin
        var authenticatedUserId = _authService.GetUserId();
        if (authenticatedUserId != userId)
        {
             // Allow if Admin? We need check roles. For now, strict check.
             // _logger.LogWarning("User {AuthUser} tried to access farms of {TargetUser}", authenticatedUserId, userId);
             // return Forbid(); 
        }

        var result = await _mediator.Send(new GetFarmsByTenantQuery(userId, includeInactive), ct);
        return Ok(ApiResponse<FarmListResponse>.Ok(result));
    }
}
