using FluentValidation;
using HealthService.Application.Commands;

namespace HealthService.Application.Commands.Validators;

public class UpdateHealthEventCommandValidator : AbstractValidator<UpdateHealthEventCommand>
{
    public UpdateHealthEventCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Health event ID must be greater than zero");

        RuleFor(x => x.Dto.Date)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("Event date cannot be in the future")
            .When(x => x.Dto.Date.HasValue);

        RuleFor(x => x.Dto.Treatment)
            .MaximumLength(500)
            .WithMessage("Treatment cannot exceed 500 characters")
            .When(x => x.Dto.Treatment != null);

        RuleFor(x => x.Dto.VeterinarianName)
            .MaximumLength(100)
            .WithMessage("Veterinarian name cannot exceed 100 characters")
            .When(x => x.Dto.VeterinarianName != null);

        RuleFor(x => x.Dto.Description)
            .MaximumLength(1000)
            .WithMessage("Description cannot exceed 1000 characters")
            .When(x => x.Dto.Description != null);
    }
}
