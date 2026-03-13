using MediatR;
using HerdService.Application.DTOs;
using HerdService.Application.Interfaces;

namespace HerdService.Application.Queries.GetBreeds;

public class GetBreedsQueryHandler : IRequestHandler<GetBreedsQuery, IEnumerable<BreedResponse>>
{
    private readonly IBreedRepository _repository;

    public GetBreedsQueryHandler(IBreedRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<BreedResponse>> Handle(GetBreedsQuery request, CancellationToken cancellationToken)
    {
        var breeds = await _repository.GetAllAsync(cancellationToken);
        return breeds.Select(b => new BreedResponse(b.Id, b.Name));
    }
}
