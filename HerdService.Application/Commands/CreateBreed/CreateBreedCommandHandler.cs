using HerdService.Application.DTOs;
using HerdService.Application.Interfaces;
using HerdService.Domain.Entities;
using MediatR;

namespace HerdService.Application.Commands.CreateBreed;

public class CreateBreedCommandHandler : IRequestHandler<CreateBreedCommand, BreedResponse>
{
    private readonly IBreedRepository _repository;

    public CreateBreedCommandHandler(IBreedRepository repository)
    {
        _repository = repository;
    }

    public async Task<BreedResponse> Handle(CreateBreedCommand request, CancellationToken cancellationToken)
    {
        var breed = new Breed
        {
            Name = request.Name
        };

        var createdBreed = await _repository.AddAsync(breed, cancellationToken);

        return new BreedResponse(createdBreed.Id, createdBreed.Name);
    }
}
