using FluentValidation;
using SalesService.Application.Commands;

namespace SalesService.Application.Commands.Validators;

public class UpdateSaleCommandValidator : AbstractValidator<UpdateSaleCommand>
{
    public UpdateSaleCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Sale ID must be greater than zero");

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
    }
}
