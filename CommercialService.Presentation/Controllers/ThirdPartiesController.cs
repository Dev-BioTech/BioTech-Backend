using CommercialService.Application.Commands;
using CommercialService.Application.DTOs;
using CommercialService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

using CommercialService.Presentation.Common;

namespace CommercialService.Presentation.Controllers;

[ApiController]
[Route("api/third-parties")]
[Authorize]
public class ThirdPartiesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly Services.IGatewayAuthenticationService _authService;

    public ThirdPartiesController(IMediator mediator, Services.IGatewayAuthenticationService authService)
    {
        _mediator = mediator;
        _authService = authService;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<long>>> Create([FromBody] CreateThirdPartyDto dto)
    {
        try
        {
            var farmId = _authService.GetFarmId();
            // Ensure DTO uses context farm
            var secureDto = dto with { FarmId = farmId }; 
            
            var id = await _mediator.Send(new CreateThirdPartyCommand(secureDto));
            return CreatedAtAction(nameof(GetById), new { id = id }, ApiResponse<long>.Ok(id, "Third party created successfully"));
        }
        catch (System.ArgumentException ex)
        {
            return BadRequest(ApiResponse<long>.Fail(ex.Message));
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Update(long id, [FromBody] UpdateThirdPartyDto dto)
    {
        // Verify ownership potentially needed here or in Handler using context farmId
        var result = await _mediator.Send(new UpdateThirdPartyCommand(id, dto));
        if (!result) return NotFound(ApiResponse<bool>.Fail("Third party not found or update failed"));
        return Ok(ApiResponse<bool>.Ok(true, "Third party updated successfully"));
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<ThirdPartyDto>>>> GetAll(
        // [FromQuery] int farmId, // Removed manual param
        [FromQuery] bool? isSupplier,
        [FromQuery] bool? isCustomer,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var farmId = _authService.GetFarmId();
        var result = await _mediator.Send(new GetThirdPartiesQuery(farmId, isSupplier, isCustomer, page, pageSize));
        return Ok(ApiResponse<List<ThirdPartyDto>>.Ok(result));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<ThirdPartyDto>>> GetById(long id)
    {
        var result = await _mediator.Send(new GetThirdPartyByIdQuery(id));
        if (result == null) 
            return NotFound(ApiResponse<ThirdPartyDto>.Fail("Third party not found"));
            
        // Strict Check
        var farmId = _authService.GetFarmId();
        if (result.FarmId != farmId)
             return NotFound(ApiResponse<ThirdPartyDto>.Fail("Third party not found"));

        return Ok(ApiResponse<ThirdPartyDto>.Ok(result));
    }
}
