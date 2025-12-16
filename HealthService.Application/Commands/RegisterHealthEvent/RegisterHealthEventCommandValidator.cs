using FluentValidation;

namespace HealthService.Application.Commands;

public class RegisterHealthEventCommandValidator : AbstractValidator<RegisterHealthEventCommand>
{
    public RegisterHealthEventCommandValidator()
    {
        RuleFor(x => x.FarmId).GreaterThan(0).WithMessage("FarmId is required");
        RuleFor(x => x.EventType).NotEmpty().WithMessage("EventType is required");
        RuleFor(x => x.EventDate).NotEmpty().LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("EventDate cannot be in the future");
            
        RuleFor(x => x)
            .Must(x => x.AnimalId.HasValue != x.BatchId.HasValue)
            .WithMessage("Either AnimalId OR BatchId must be provided, but not both.");
            
        RuleFor(x => x.Cost)
            .GreaterThanOrEqualTo(0).When(x => x.Cost.HasValue)
            .WithMessage("Cost cannot be negative");
            
        var validTypes = new[] { "Vaccination", "Treatment", "Diagnosis", "Check-up", "Medication", "Surgery", "Injury", "Disease" };
        RuleFor(x => x.EventType)
            .Must(type => validTypes.Contains(type))
            .WithMessage($"Invalid EventType. Must be one of: {string.Join(", ", validTypes)}");
    }
}
