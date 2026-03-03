using FluentValidation;
using InventoryService.Application.Commands;

namespace InventoryService.Application.Commands.Validators;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Product ID must be greater than zero");

        RuleFor(x => x.Dto.Name)
            .NotEmpty()
            .WithMessage("Product name is required")
            .MaximumLength(100)
            .WithMessage("Product name cannot exceed 100 characters");

        RuleFor(x => x.Dto.UnitOfMeasure)
            .MaximumLength(20)
            .WithMessage("Unit of measure cannot exceed 20 characters")
            .When(x => !string.IsNullOrEmpty(x.Dto.UnitOfMeasure));

        RuleFor(x => x.Dto.MinimumStock)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Minimum stock cannot be negative")
            .When(x => x.Dto.MinimumStock.HasValue);
    }
}
