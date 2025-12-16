using MediatR;
using Microsoft.AspNetCore.Mvc;
using HealthService.Application.Commands.CreateHealthEvent;
using HealthService.Application.DTOs;

namespace HealthService.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthEventController : ControllerBase
{
    private readonly ISender _sender;

    public HealthEventController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<ActionResult<HealthEventResponse>> CreateHealthEvent([FromBody] CreateHealthEventCommand command)
    {
        var response = await _sender.Send(command);
        return CreatedAtAction(nameof(CreateHealthEvent), new { id = response.Id }, response);
    }
    [HttpGet("farm/{farmId}")]
    public async Task<ActionResult<List<HealthEventResponse>>> GetByFarm(int farmId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _sender.Send(new HealthService.Application.Queries.GetHealthEventsByFarm.GetHealthEventsByFarmQuery(farmId, page, pageSize));
        return Ok(result);
    }

    [HttpGet("animal/{animalId}")]
    public async Task<ActionResult<List<HealthEventResponse>>> GetByAnimal(long animalId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _sender.Send(new HealthService.Application.Queries.GetHealthEventsByAnimal.GetHealthEventsByAnimalQuery(animalId, page, pageSize));
        return Ok(result);
    }

    [HttpGet("batch/{batchId}")]
    public async Task<ActionResult<List<HealthEventResponse>>> GetByBatch(int batchId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _sender.Send(new HealthService.Application.Queries.GetHealthEventsByBatch.GetHealthEventsByBatchQuery(batchId, page, pageSize));
        return Ok(result);
    }

    [HttpGet("type/{type}")]
    public async Task<ActionResult<List<HealthEventResponse>>> GetByType(string type, [FromQuery] int farmId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _sender.Send(new HealthService.Application.Queries.GetHealthEventsByType.GetHealthEventsByTypeQuery(type, farmId, page, pageSize));
        return Ok(result);
    }
}
