using FluentValidation;
using RealEstate.Contracts.DTOs;

namespace Hotels.Application.Validators
{
    public class PropertyDtoValidator : AbstractValidator<PropertyDto>
    {
        public PropertyDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Property name is required.");
            RuleFor(x => x.Address).NotEmpty().WithMessage("Address is required.");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0.");
            RuleFor(x => x.Owner).NotNull().WithMessage("Owner is required.");
        }
    }
}