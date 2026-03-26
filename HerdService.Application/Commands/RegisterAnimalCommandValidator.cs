using FluentValidation;

namespace HerdService.Application.Commands;

public class RegisterAnimalCommandValidator : AbstractValidator<RegisterAnimalCommand>
{
    public RegisterAnimalCommandValidator()
    {
        RuleFor(x => x.VisualCode).NotEmpty().MaximumLength(20);
        RuleFor(x => x.FarmId).GreaterThan(0);
        RuleFor(x => x.BreedId).GreaterThan(0).When(x => x.BreedId.HasValue);
        RuleFor(x => x.CategoryId).GreaterThan(0).When(x => x.CategoryId.HasValue);
        RuleFor(x => x.BirthDate).LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow));
        RuleFor(x => x.Sex).Must(x => x == "M" || x == "F").WithMessage("Sex must be 'M' or 'F'");
        RuleFor(x => x.Weight).GreaterThan(0).When(x => x.Weight.HasValue);
        RuleFor(x => x.Height).GreaterThan(0).When(x => x.Height.HasValue);
    }
}
