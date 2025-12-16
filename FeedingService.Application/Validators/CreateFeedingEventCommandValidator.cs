using FeedingService.Application.Commands.CreateFeedingEvent;
using FluentValidation;

namespace FeedingService.Application.Validators;

public class CreateFeedingEventCommandValidator : AbstractValidator<CreateFeedingEventCommand>
{
    public CreateFeedingEventCommandValidator()
    {
        RuleFor(x => x.FarmId)
            .GreaterThan(0)
            .WithMessage("FarmId must be greater than zero");

        RuleFor(x => x.SupplyDate)
            .NotEmpty()
            .LessThanOrEqualTo(DateTime.UtcNow.Date)
            .WithMessage("SupplyDate cannot be in the future");

        RuleFor(x => x.ProductId)
            .GreaterThan(0)
            .WithMessage("ProductId must be greater than zero");

        RuleFor(x => x.TotalQuantity)
            .GreaterThan(0)
            .WithMessage("TotalQuantity must be greater than zero");

        RuleFor(x => x.AnimalsFedCount)
            .GreaterThan(0)
            .WithMessage("AnimalsFedCount must be greater than zero");

        RuleFor(x => x.UnitCostAtMoment)
            .GreaterThanOrEqualTo(0)
            .WithMessage("UnitCostAtMoment cannot be negative");

        RuleFor(x => x)
            .Must(x => x.BatchId.HasValue || x.AnimalId.HasValue)
            .WithMessage("Either BatchId or AnimalId must be provided");

        RuleFor(x => x)
            .Must(x => !(x.BatchId.HasValue && x.AnimalId.HasValue))
            .WithMessage("Cannot specify both BatchId and AnimalId");

        RuleFor(x => x.BatchId)
            .GreaterThan(0)
            .When(x => x.BatchId.HasValue)
            .WithMessage("BatchId must be greater than zero when provided");

        RuleFor(x => x.AnimalId)
            .GreaterThan(0)
            .When(x => x.AnimalId.HasValue)
            .WithMessage("AnimalId must be greater than zero when provided");

        RuleFor(x => x.Observations)
            .MaximumLength(2000)
            .When(x => !string.IsNullOrWhiteSpace(x.Observations))
            .WithMessage("Observations cannot exceed 2000 characters");
    }
}
