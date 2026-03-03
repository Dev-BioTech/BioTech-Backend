using MediatR;
using HerdService.Application.Interfaces;
using HerdService.Application.DTOs;
using HerdService.Domain.Entities;

namespace HerdService.Application.Commands;

public class CreateBreedCommandHandler : IRequestHandler<CreateBreedCommand, BreedResponse>
{
    private readonly IBreedRepository _breedRepository;

    public CreateBreedCommandHandler(IBreedRepository breedRepository)
    {
        _breedRepository = breedRepository;
    }

    public async Task<BreedResponse> Handle(CreateBreedCommand request, CancellationToken cancellationToken)
    {
        var breed = new Breed(request.Name);
        
        var createdBreed = await _breedRepository.AddAsync(breed, cancellationToken);
        
        return new BreedResponse(
            createdBreed.Id,
            createdBreed.Name
        );
    }
}
