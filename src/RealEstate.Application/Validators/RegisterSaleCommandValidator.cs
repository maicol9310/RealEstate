using FluentValidation;
using RealEstate.Application.Commands;

public class RegisterSaleCommandValidator : AbstractValidator<RegisterSaleCommand>
{
    public RegisterSaleCommandValidator()
    {
        RuleFor(x => x.dateSale)
            .NotEmpty().WithMessage("Sale date is required")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Sale date cannot be in the future");

        RuleFor(x => x.name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");

        RuleFor(x => x.value)
            .GreaterThan(0).WithMessage("Value must be greater than 0");

        RuleFor(x => x.tax)
            .GreaterThanOrEqualTo(0).WithMessage("Tax must be 0 or greater");

        RuleFor(x => x.idProperty)
            .NotEmpty().WithMessage("Property ID is required");
    }
}