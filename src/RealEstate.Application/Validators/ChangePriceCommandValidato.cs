using FluentValidation;
using RealEstate.Application.Commands;
using System;

public class ChangePriceCommandValidator : AbstractValidator<ChangePriceCommand>
{
    public ChangePriceCommandValidator()
    {
        RuleFor(x => x.PropertyId)
            .NotEmpty().WithMessage("PropertyId is required");

        RuleFor(x => x.NewPrice)
            .GreaterThan(0).WithMessage("NewPrice must be greater than 0");
    }
}