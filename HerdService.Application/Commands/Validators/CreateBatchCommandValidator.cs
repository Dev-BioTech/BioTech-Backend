using FluentValidation;
using HerdService.Application.Commands;

namespace HerdService.Application.Commands.Validators;

public class CreateBatchCommandValidator : AbstractValidator<CreateBatchCommand>
{
    public CreateBatchCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Batch name is required")
            .MaximumLength(100)
            .WithMessage("Batch name cannot exceed 100 characters");
            
        RuleFor(x => x.FarmId)
            .GreaterThan(0)
            .WithMessage("Valid FarmId is required");
    }
}
