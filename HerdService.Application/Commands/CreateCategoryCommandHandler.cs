using MediatR;
using HerdService.Application.Interfaces;
using HerdService.Application.DTOs;
using HerdService.Domain.Entities;

namespace HerdService.Application.Commands;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, AnimalCategoryResponse>
{
    private readonly IAnimalCategoryRepository _categoryRepository;

    public CreateCategoryCommandHandler(IAnimalCategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<AnimalCategoryResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = new AnimalCategory(request.Name);
        
        var createdCategory = await _categoryRepository.AddAsync(category, cancellationToken);
        
        return new AnimalCategoryResponse(
            createdCategory.Id,
            createdCategory.Name
        );
    }
}
