using ReproductionService.Application.DTOs;
using MediatR;

namespace ReproductionService.Application.Commands;

public record RegisterBirthCommand(long MotherAnimalId, string OffspringTag, decimal Weight, string Gender, DateTime BirthDate) : IRequest<BirthDto>;
