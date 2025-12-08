using FluentValidation;
using RealEstate.Application.Commands;
using RealEstate.Application.Interfaces;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    private readonly IUnitOfWork _uow;

    public LoginCommandValidator(IUnitOfWork uow)
    {
        _uow = uow;

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required");

        RuleFor(x => x)
            .MustAsync(async (cmd, ct) =>
            {
                var user = await _uow.Users.GetByUsernameAsync(cmd.Username, ct);
                return user != null;
            })
            .WithMessage("Incorrect username or password")
            .When(x => !string.IsNullOrWhiteSpace(x.Username)); // Solo si username no está vacío

        RuleFor(x => x)
            .MustAsync(async (cmd, ct) =>
            {
                var user = await _uow.Users.GetByUsernameAsync(cmd.Username, ct);
                return user != null && user.PasswordHash == cmd.Password;
            })
            .WithMessage("Incorrect username or password")
            .When(x => !string.IsNullOrWhiteSpace(x.Username) && !string.IsNullOrWhiteSpace(x.Password));

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required");
    }
}
