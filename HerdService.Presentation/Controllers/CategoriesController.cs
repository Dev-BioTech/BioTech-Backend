using HerdService.Application.Commands.CreateCategory;
using HerdService.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HerdService.Presentation.Controllers;

[ApiController]
[Route("api/v1/categories")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<AnimalCategoryResponse>> Create(CreateCategoryCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(Create), new { id = result.Id }, result);
    }
}
