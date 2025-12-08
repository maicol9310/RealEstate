using FluentValidation;
using RealEstate.Contracts.DTOs;

namespace Hotels.Application.Validators
{
    public class OwnerDtoValidator : AbstractValidator<OwnerDto>
    {
        public OwnerDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.Address).NotEmpty().WithMessage("Address is required.");
            RuleFor(x => x.Photo).NotEmpty().WithMessage("Photo is required.");
        }
    }
}
