using MediatR;
using HerdService.Application.Interfaces;
using HerdService.Application.DTOs;

namespace HerdService.Application.Queries;

public class GetAllBreedsQueryHandler : IRequestHandler<GetAllBreedsQuery, IEnumerable<BreedResponse>>
{
    private readonly IBreedRepository _breedRepository;

    public GetAllBreedsQueryHandler(IBreedRepository breedRepository)
    {
        _breedRepository = breedRepository;
    }

    public async Task<IEnumerable<BreedResponse>> Handle(GetAllBreedsQuery request, CancellationToken cancellationToken)
    {
        var breeds = await _breedRepository.GetAllAsync(cancellationToken);
        
        return breeds.Select(breed => new BreedResponse(
            breed.Id,
            breed.Name
        ));
    }
}
