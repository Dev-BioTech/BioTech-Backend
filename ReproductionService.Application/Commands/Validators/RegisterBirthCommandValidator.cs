using FluentValidation;
using ReproductionService.Application.Commands;

namespace ReproductionService.Application.Commands.Validators;

public class RegisterBirthCommandValidator : AbstractValidator<RegisterBirthCommand>
{
    public RegisterBirthCommandValidator()
    {
        RuleFor(x => x.MotherAnimalId)
            .GreaterThan(0)
            .WithMessage("MotherAnimalId must be greater than zero");
            
        RuleFor(x => x.OffspringTag)
            .NotEmpty()
            .WithMessage("OffspringTag is required")
            .MaximumLength(50)
            .WithMessage("OffspringTag cannot exceed 50 characters");
            
        RuleFor(x => x.Weight)
            .GreaterThan(0)
            .WithMessage("Weight must be greater than zero");
            
        RuleFor(x => x.Gender)
            .Must(gender => gender == "Male" || gender == "Female")
            .WithMessage("Gender must be 'Male' or 'Female'");
            
        RuleFor(x => x.BirthDate)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("BirthDate cannot be in the future");
    }
}
