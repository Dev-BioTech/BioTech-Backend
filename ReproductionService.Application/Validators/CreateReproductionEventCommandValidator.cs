using FluentValidation;
using ReproductionService.Application.Commands.CreateReproductionEvent;
using ReproductionService.Domain.Enums;

namespace ReproductionService.Application.Validators;

public class CreateReproductionEventCommandValidator : AbstractValidator<CreateReproductionEventCommand>
{
    public CreateReproductionEventCommandValidator()
    {
        RuleFor(x => x.FarmId)
            .GreaterThan(0)
            .WithMessage("FarmId must be greater than zero");

        RuleFor(x => x.AnimalId)
            .GreaterThan(0)
            .WithMessage("AnimalId must be greater than zero");

        RuleFor(x => x.EventDate)
            .LessThanOrEqualTo(DateTime.UtcNow.Date)
            .WithMessage("EventDate cannot be in the future");

        RuleFor(x => x.EventType)
            .IsInEnum()
            .WithMessage("Invalid EventType");

        RuleFor(x => x.SemenBatchId)
            .NotNull()
            .When(x => x.EventType == ReproductionEventType.Insemination)
            .WithMessage("SemenBatchId is required for Insemination");

        RuleFor(x => x.MaleAnimalId)
            .NotNull()
            .When(x => x.EventType == ReproductionEventType.NaturalMating)
            .WithMessage("MaleAnimalId is required for NaturalMating");

        RuleFor(x => x.PregnancyResult)
            .NotNull()
            .When(x => x.EventType == ReproductionEventType.PregnancyCheck)
            .WithMessage("PregnancyResult is required for PregnancyCheck");

        RuleFor(x => x.OffspringCount)
            .GreaterThanOrEqualTo(1)
            .When(x => x.EventType == ReproductionEventType.Birth)
            .WithMessage("OffspringCount must be at least 1 for Birth");

        RuleFor(x => x.Observations)
            .MaximumLength(2000)
            .When(x => !string.IsNullOrEmpty(x.Observations))
            .WithMessage("Observations cannot exceed 2000 characters");
    }
}
