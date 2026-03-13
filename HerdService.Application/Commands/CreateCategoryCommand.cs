using HerdService.Application.DTOs;
using MediatR;

namespace HerdService.Application.Commands;

public record CreateCategoryCommand(string Name) : IRequest<AnimalCategoryResponse>;
