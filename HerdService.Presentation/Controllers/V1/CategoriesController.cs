using MediatR;
using Microsoft.AspNetCore.Mvc;
using HerdService.Application.Queries.GetCategories;
using HerdService.Application.DTOs;
using Shared.Infrastructure.Common;

namespace HerdService.Presentation.Controllers.V1;

[ApiController]
[Route("api/v1/categories")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<AnimalCategoryResponse>>>> GetAll()
    {
        var result = await _mediator.Send(new GetCategoriesQuery());
        return Ok(ApiResponse<IEnumerable<AnimalCategoryResponse>>.Ok(result));
    }
}
