using FluentValidation;
using SalesService.Application.Commands;

namespace SalesService.Application.Commands.Validators;

public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
{
    public CreateSaleCommandValidator()
    {
        RuleFor(x => x.Dto.FarmId)
            .GreaterThan(0)
            .WithMessage("FarmId must be greater than zero");

        RuleFor(x => x.Dto.BuyerName)
            .NotEmpty()
            .WithMessage("BuyerName is required")
            .MaximumLength(100)
            .WithMessage("BuyerName cannot exceed 100 characters");

        RuleFor(x => x.Dto.Amount)
            .GreaterThan(0)
            .WithMessage("Amount must be greater than zero");

        RuleFor(x => x.Dto.SaleDate)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("SaleDate cannot be in the future");

        RuleFor(x => x.Dto.AnimalId)
            .GreaterThan(0)
            .WithMessage("AnimalId must be greater than zero")
            .When(x => x.Dto.AnimalId.HasValue);
    }
}
