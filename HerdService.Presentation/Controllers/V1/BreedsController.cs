using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using HerdService.Application.Commands;
using HerdService.Application.Queries;
using HerdService.Application.DTOs;
using HerdService.Presentation.Services;
using HerdService.Presentation.Common;

namespace HerdService.Presentation.Controllers.V1;

[ApiController]
[Route("api/v1/[controller]")]
public class BreedsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly GatewayAuthenticationService _authService;

    public BreedsController(IMediator mediator, GatewayAuthenticationService authService)
    {
        _mediator = mediator;
        _authService = authService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<BreedResponse>>>> GetAllBreeds()
    {
        try
        {
            var result = await _mediator.Send(new GetAllBreedsQuery());
            return Ok(ApiResponse<IEnumerable<BreedResponse>>.Ok(result, "Breeds retrieved successfully"));
        }
        catch (Exception)
        {
            return StatusCode(500, ApiResponse<IEnumerable<BreedResponse>>.Fail("Internal server error"));
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<BreedResponse>>> CreateBreed([FromBody] CreateBreedCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetAllBreeds), null, ApiResponse<BreedResponse>.Ok(result, "Breed created successfully"));
        }
        catch (ValidationException ex)
        {
            return BadRequest(ApiResponse<BreedResponse>.Fail("Validation failed", ex.Errors.Select(e => e.ErrorMessage)));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponse<BreedResponse>.Fail(ex.Message));
        }
        catch (Exception)
        {
            return StatusCode(500, ApiResponse<BreedResponse>.Fail("Internal server error"));
        }
    }
}
