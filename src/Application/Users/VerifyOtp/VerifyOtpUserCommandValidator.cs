using FluentValidation;

namespace Application.Users.VerifyOtp;

public sealed class VerifyOtpUserCommandValidator : AbstractValidator<VerifyOtpUserCommand>
{
    public VerifyOtpUserCommandValidator()
    {
        RuleFor(command => command.ClientId)
            .NotEmpty();

        RuleFor(command => command.Otp)
            .NotEmpty()
            .Length(6);
    }
}