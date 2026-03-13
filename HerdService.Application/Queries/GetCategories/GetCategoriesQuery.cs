using MediatR;
using HerdService.Application.DTOs;

namespace HerdService.Application.Queries.GetCategories;

public record GetCategoriesQuery() : IRequest<IEnumerable<AnimalCategoryResponse>>;
