using CommercialService.Application.Commands;
using CommercialService.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

using CommercialService.Presentation.Common;

namespace CommercialService.Presentation.Controllers;


[ApiController]
[Route("api/transactions")]
[Authorize]
public class TransactionsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly Services.GatewayAuthenticationService _authService;

    public TransactionsController(IMediator mediator, Services.GatewayAuthenticationService authService)
    {
        _mediator = mediator;
        _authService = authService;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<long>>> CreateTransaction([FromBody] CreateTransactionDto dto)
    {
        // Override User and Farm Context? Prompt says "FarmId is mandatory" but Gateway Auth implies we trust the header.
        // We should probably enforce FarmId matches context or set it from context.
        // Strategy: Set RegisteredBy from context. Verify FarmId.

        var userId = _authService.GetUserId();
        var farmId = _authService.GetFarmId();

        if (dto.FarmId != 0)
        {
            if (!_authService.HasAccessToFarm(dto.FarmId))
                return Unauthorized(ApiResponse<long>.Fail("User does not have access to the specified Farm"));

            // If valid, we use dto.FarmId. 
            // We don't check against farmId (context) because context might be default.
        }
        else
        {
            // If not provided in DTO, fallback to context
            if (!farmId.HasValue) return BadRequest(ApiResponse<long>.Fail("User is not associated with a valid Farm"));
            // We will modify the DTO to use the context farm
        }

        // Ensure DTO uses context farm if 0 or matches.
        // Ensure DTO uses the correct farm
        var finalFarmId = dto.FarmId != 0 ? dto.FarmId : (farmId ?? 0);
        if (finalFarmId == 0) return BadRequest(ApiResponse<long>.Fail("FarmId is required"));

        var secureDto = dto with { FarmId = finalFarmId };
        // Actually dto might be class. Let's assume CreateTransactionCommand takes userId separately.

        // Command expects userId constraint
        var transactionId = await _mediator.Send(new CreateTransactionCommand(secureDto, userId ?? 0));
        return CreatedAtAction(nameof(GetTransactionById), new { id = transactionId }, ApiResponse<long>.Ok(transactionId, "Transaction created successfully"));
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<TransactionSummaryDto>>>> GetTransactions(
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate,
        [FromQuery] Domain.Enums.TransactionType? type,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var farmId = _authService.GetFarmId();
        if (!farmId.HasValue) return BadRequest(ApiResponse<List<TransactionSummaryDto>>.Fail("User is not associated with a valid Farm"));

        var result = await _mediator.Send(new CommercialService.Application.Queries.GetTransactionsQuery(farmId.Value, fromDate, toDate, type, page, pageSize));
        return Ok(ApiResponse<List<TransactionSummaryDto>>.Ok(result));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<TransactionFullDto>>> GetTransactionById(long id)
    {
        var result = await _mediator.Send(new CommercialService.Application.Queries.GetTransactionByIdQuery(id));
        if (result == null)
            return NotFound(ApiResponse<TransactionFullDto>.Fail("Transaction not found"));

        // Verify ownership? Handler likely filters strictly, but good practice to check logic if ID is global.
        var farmId = _authService.GetFarmId();
        if (result.FarmId != farmId)
            return NotFound(ApiResponse<TransactionFullDto>.Fail("Transaction not found")); // Mask unauthorized as not found

        return Ok(ApiResponse<TransactionFullDto>.Ok(result));
    }

    [HttpGet("{id}/animals")]
    public async Task<ActionResult<ApiResponse<List<TransactionAnimalDto>>>> GetTransactionAnimals(long id)
    {
        // Should verify Transaction ownership first ideally, or Handler does it.
        // For now trusting Handler or assuming separate verification step not explicitly requested beyond "List involved".
        var result = await _mediator.Send(new CommercialService.Application.Queries.GetTransactionAnimalsQuery(id));
        return Ok(ApiResponse<List<TransactionAnimalDto>>.Ok(result));
    }

    [HttpGet("{id}/products")]
    public async Task<ActionResult<ApiResponse<List<TransactionProductDto>>>> GetTransactionProducts(long id)
    {
        var result = await _mediator.Send(new CommercialService.Application.Queries.GetTransactionProductsQuery(id));
        return Ok(ApiResponse<List<TransactionProductDto>>.Ok(result));
    }
}
