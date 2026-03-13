using FluentValidation;
using HerdService.Application.Commands;

namespace HerdService.Application.Commands.Validators;

public class CreateBreedCommandValidator : AbstractValidator<CreateBreedCommand>
{
    public CreateBreedCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Breed name is required")
            .MaximumLength(100)
            .WithMessage("Breed name cannot exceed 100 characters");
    }
}
