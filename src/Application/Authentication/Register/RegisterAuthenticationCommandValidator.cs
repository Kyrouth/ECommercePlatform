using Domain.ValueObjects;
using FluentValidation;

namespace Application.Authentication.Register;

public sealed class RegisterAuthenticationCommandValidator : AbstractValidator<RegisterAuthenticationCommand>
{
    public RegisterAuthenticationCommandValidator()
    {
        RuleFor(rac => rac.VerificationTokenId)
            .NotEmpty();

        RuleFor(rac => rac.FirstName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(rac => rac.LastName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(rac => rac.Username)
            .NotEmpty()
            .MinimumLength(Username.MinLength)
            .MaximumLength(Username.MaxLength)
            .Matches(Username.Pattern);
    }
}