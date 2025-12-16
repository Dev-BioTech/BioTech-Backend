using FluentValidation;

namespace AuthService.Application.Commands.CreateFarm;

public class CreateFarmCommandValidator : AbstractValidator<CreateFarmCommand>
{
    public CreateFarmCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Farm name is required")
            .MaximumLength(200).WithMessage("Farm name cannot exceed 200 characters");

        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .When(x => x.UserId.HasValue)
            .WithMessage("Valid User ID is required");
    }
}
