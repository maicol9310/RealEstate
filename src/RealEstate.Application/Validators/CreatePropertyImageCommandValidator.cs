using FluentValidation;
using RealEstate.Application.Commands;
using System;

public class CreatePropertyImageCommandValidator : AbstractValidator<CreatePropertyImageCommand>
{
    public CreatePropertyImageCommandValidator()
    {
        RuleFor(x => x.PropertyId)
            .NotEmpty().WithMessage("PropertyId is required");

        RuleFor(x => x.ImageBase64)
            .NotEmpty().WithMessage("ImageBase64 is required")
            .Must(BeAValidBase64).WithMessage("ImageBase64 must be a valid Base64 string");
    }

    private bool BeAValidBase64(string base64)
    {
        if (string.IsNullOrWhiteSpace(base64)) return false;

        Span<byte> buffer = new Span<byte>(new byte[base64.Length]);
        return Convert.TryFromBase64String(base64, buffer, out _);
    }
}