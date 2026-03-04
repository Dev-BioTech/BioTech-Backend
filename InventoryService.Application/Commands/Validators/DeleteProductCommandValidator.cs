using FluentValidation;
using InventoryService.Application.Commands;

namespace InventoryService.Application.Commands.Validators;

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Product ID must be greater than zero");
    }
}
