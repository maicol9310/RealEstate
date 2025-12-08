using FluentValidation;
using RealEstate.Application.Commands;
using System;

public class UpdatePropertyCommandValidator : AbstractValidator<UpdatePropertyCommand>
{
    public UpdatePropertyCommandValidator()
    {
        RuleFor(x => x.PropertyId)
            .NotEmpty().WithMessage("PropertyId is required");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required")
            .MaximumLength(200).WithMessage("Address cannot exceed 200 characters");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0");

        RuleFor(x => x.CodeInternal)
            .NotEmpty().WithMessage("CodeInternal is required")
            .MaximumLength(50).WithMessage("CodeInternal cannot exceed 50 characters");

        RuleFor(x => x.Year)
            .GreaterThan(1900).WithMessage("Year must be valid");

        RuleFor(x => x.IdOwner)
            .NotEmpty().WithMessage("IdOwner is required");
    }
}