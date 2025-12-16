using InventoryService.Application.Commands;
using InventoryService.Application.DTOs;
using InventoryService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryService.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnimalsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AnimalsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<long>> RegisterAnimal(CreateAnimalDto dto)
    {
        try
        {
            var id = await _mediator.Send(new RegisterAnimalCommand(dto));
            return CreatedAtAction(nameof(GetAnimals), new { farmId = dto.FarmId }, id);
        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AnimalDto>>> GetAnimals([FromQuery] int farmId)
    {
        var animals = await _mediator.Send(new GetAnimalsByFarmQuery(farmId));
        return Ok(animals);
    }
}
