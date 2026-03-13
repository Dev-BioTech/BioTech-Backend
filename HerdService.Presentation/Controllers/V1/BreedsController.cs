using MediatR;
using Microsoft.AspNetCore.Mvc;
using HerdService.Application.Queries.GetBreeds;
using HerdService.Application.DTOs;
using Shared.Infrastructure.Common;

namespace HerdService.Presentation.Controllers.V1;

[ApiController]
[Route("api/v1/breeds")]
public class BreedsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BreedsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<BreedResponse>>>> GetAll()
    {
        var result = await _mediator.Send(new GetBreedsQuery());
        return Ok(ApiResponse<IEnumerable<BreedResponse>>.Ok(result));
    }
}
