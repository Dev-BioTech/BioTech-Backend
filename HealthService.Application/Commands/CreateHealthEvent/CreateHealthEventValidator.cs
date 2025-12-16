using FluentValidation;

namespace HealthService.Application.Commands.CreateHealthEvent;

public class CreateHealthEventValidator : AbstractValidator<CreateHealthEventCommand>
{
    public CreateHealthEventValidator()
    {
        RuleFor(x => x.FarmId)
            .GreaterThan(0).WithMessage("FarmId is required.");

        RuleFor(x => x.EventType)
            .NotEmpty().WithMessage("EventType is required.")
            .Must(type => new[] { "VACCINATION", "DEWORMING", "TREATMENT", "LAB_TEST" }.Contains(type))
            .WithMessage("Invalid EventType.");

        RuleFor(x => x.ServiceCost)
            .GreaterThanOrEqualTo(0).WithMessage("ServiceCost cannot be negative.");

        RuleFor(x => x)
            .Must(x => x.BatchId.HasValue || x.AnimalId.HasValue)
            .WithMessage("Either BatchId or AnimalId must be provided.");
    }
}
