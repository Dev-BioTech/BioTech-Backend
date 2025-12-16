using CommercialService.Application.Commands;
using CommercialService.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace CommercialService.Presentation.Controllers;

[ApiController]
[Route("api/transactions")]
[Authorize]
public class TransactionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TransactionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<long>> CreateTransaction([FromBody] CreateTransactionDto dto)
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
        {
            return Unauthorized("User ID not found in token.");
        }

        var transactionId = await _mediator.Send(new CreateTransactionCommand(dto, userId));
        return CreatedAtAction(nameof(CreateTransaction), new { id = transactionId }, transactionId);
    }

    [HttpGet]
    public async Task<ActionResult<List<TransactionSummaryDto>>> GetTransactions(
        [FromQuery] int farmId,
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate,
        [FromQuery] Domain.Enums.TransactionType? type)
    {
        var result = await _mediator.Send(new CommercialService.Application.Queries.GetTransactionsQuery(farmId, fromDate, toDate, type));
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TransactionFullDto>> GetTransactionById(long id)
    {
        var result = await _mediator.Send(new CommercialService.Application.Queries.GetTransactionByIdQuery(id));
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpGet("{id}/animals")]
    public async Task<ActionResult<List<TransactionAnimalDto>>> GetTransactionAnimals(long id)
    {
        var result = await _mediator.Send(new CommercialService.Application.Queries.GetTransactionAnimalsQuery(id));
        return Ok(result);
    }

    [HttpGet("{id}/products")]
    public async Task<ActionResult<List<TransactionProductDto>>> GetTransactionProducts(long id)
    {
        var result = await _mediator.Send(new CommercialService.Application.Queries.GetTransactionProductsQuery(id));
        return Ok(result);
    }
}
