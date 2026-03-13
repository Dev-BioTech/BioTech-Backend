using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using HerdService.Application.Commands;
using HerdService.Application.Queries;
using HerdService.Application.DTOs;
using HerdService.Presentation.Services;
using Shared.Infrastructure.Common;

namespace HerdService.Presentation.Controllers.V1;

[ApiController]
[Route("api/v1/categories")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly GatewayAuthenticationService _authService;

    public CategoriesController(IMediator mediator, GatewayAuthenticationService authService)
    {
        _mediator = mediator;
        _authService = authService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<AnimalCategoryResponse>>>> GetAllCategories()
    {
        try
        {
            var result = await _mediator.Send(new GetAllCategoriesQuery());
            return Ok(ApiResponse<IEnumerable<AnimalCategoryResponse>>.Ok(result, "Categories retrieved successfully"));
        }
        catch (Exception)
        {
            return StatusCode(500, ApiResponse<IEnumerable<AnimalCategoryResponse>>.Fail("Internal server error"));
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<AnimalCategoryResponse>>> CreateCategory([FromBody] CreateCategoryCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetAllCategories), null, ApiResponse<AnimalCategoryResponse>.Ok(result, "Category created successfully"));
        }
        catch (ValidationException ex)
        {
            return BadRequest(ApiResponse<AnimalCategoryResponse>.Fail("Validation failed", ex.Errors.Select(e => e.ErrorMessage)));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponse<AnimalCategoryResponse>.Fail(ex.Message));
        }
        catch (Exception)
        {
            return StatusCode(500, ApiResponse<AnimalCategoryResponse>.Fail("Internal server error"));
        }
    }
}
