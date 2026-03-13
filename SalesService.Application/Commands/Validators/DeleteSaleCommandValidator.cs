using FluentValidation;
using SalesService.Application.Commands;

namespace SalesService.Application.Commands.Validators;

public class DeleteSaleCommandValidator : AbstractValidator<DeleteSaleCommand>
{
    public DeleteSaleCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Sale ID must be greater than zero");
    }
}
