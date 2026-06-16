using FluentValidation;

namespace Application.Authentication.VerifyOtp;

public sealed class VerifyOtpAuthenticationCommandValidator : AbstractValidator<VerifyOtpAuthenticationCommand>
{
    public VerifyOtpAuthenticationCommandValidator()
    {
        RuleFor(command => command.ClientId)
            .NotEmpty();

        RuleFor(command => command.Otp)
            .NotEmpty()
            .Length(6);
    }
}