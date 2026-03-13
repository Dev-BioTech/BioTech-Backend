using HerdService.Application.DTOs;
using MediatR;

namespace HerdService.Application.Queries;

public record GetAllCategoriesQuery : IRequest<IEnumerable<AnimalCategoryResponse>>;
