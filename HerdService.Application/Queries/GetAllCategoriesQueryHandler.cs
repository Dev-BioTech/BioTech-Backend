using MediatR;
using HerdService.Application.Interfaces;
using HerdService.Application.DTOs;

namespace HerdService.Application.Queries;

public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, IEnumerable<AnimalCategoryResponse>>
{
    private readonly IAnimalCategoryRepository _categoryRepository;

    public GetAllCategoriesQueryHandler(IAnimalCategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<AnimalCategoryResponse>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAllAsync(cancellationToken);
        
        return categories.Select(category => new AnimalCategoryResponse(
            category.Id,
            category.Name
        ));
    }
}
