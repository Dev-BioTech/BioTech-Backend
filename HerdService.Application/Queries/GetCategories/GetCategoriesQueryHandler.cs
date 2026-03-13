using MediatR;
using HerdService.Application.DTOs;
using HerdService.Application.Interfaces;

namespace HerdService.Application.Queries.GetCategories;

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, IEnumerable<AnimalCategoryResponse>>
{
    private readonly IAnimalCategoryRepository _repository;

    public GetCategoriesQueryHandler(IAnimalCategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<AnimalCategoryResponse>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _repository.GetAllAsync(cancellationToken);
        return categories.Select(c => new AnimalCategoryResponse(c.Id, c.Name));
    }
}
