using FluentValidation;
using HerdService.Application.Commands;

namespace HerdService.Application.Commands.Validators;

public class CreatePaddockCommandValidator : AbstractValidator<CreatePaddockCommand>
{
    public CreatePaddockCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Paddock name is required")
            .MaximumLength(100)
            .WithMessage("Paddock name cannot exceed 100 characters");
            
        RuleFor(x => x.FarmId)
            .GreaterThan(0)
            .WithMessage("Valid FarmId is required");
    }
}
