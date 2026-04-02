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

        When(x => x.Dto.BuyerName != null, () => {
            RuleFor(x => x.Dto.BuyerName)
                .NotEmpty()
                .WithMessage("BuyerName cannot be empty")
                .MaximumLength(100)
                .WithMessage("BuyerName cannot exceed 100 characters");
        });

        When(x => x.Dto.Amount != null, () => {
            RuleFor(x => x.Dto.Amount)
                .GreaterThan(0)
                .WithMessage("Amount must be greater than zero");
        });

        When(x => x.Dto.SaleDate != null, () => {
            RuleFor(x => x.Dto.SaleDate)
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("SaleDate cannot be in the future");
        });
    }
}
