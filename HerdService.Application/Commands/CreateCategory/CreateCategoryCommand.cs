using HerdService.Application.DTOs;
using MediatR;

namespace HerdService.Application.Commands.CreateCategory;

public record CreateCategoryCommand(string Name) : IRequest<AnimalCategoryResponse>;
