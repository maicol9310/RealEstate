using FluentValidation;
using RealEstate.Application.Commands;

namespace Hotels.Application.Validators
{
    public class CreatePropertyCommandValidator : AbstractValidator<CreatePropertyCommand>
    {
        public CreatePropertyCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Property name is required.");
            RuleFor(x => x.Address).NotEmpty().WithMessage("Address is required.");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0.");
            RuleFor(x => x.IdOwner).NotNull().WithMessage("Owner is required.");
        }
    }
}