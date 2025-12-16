using HerdService.Application.DTOs;
using HerdService.Application.Interfaces;
using HerdService.Domain.Entities;
using MediatR;

namespace HerdService.Application.Commands.CreateCategory;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, AnimalCategoryResponse>
{
    private readonly IAnimalCategoryRepository _repository;

    public CreateCategoryCommandHandler(IAnimalCategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<AnimalCategoryResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = new AnimalCategory
        {
            Name = request.Name
        };

        var createdCategory = await _repository.AddAsync(category, cancellationToken);

        return new AnimalCategoryResponse(createdCategory.Id, createdCategory.Name);
    }
}
