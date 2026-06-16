using FluentValidation;

namespace Application.Authentication.ResendOtp;

public sealed class ResendOtpAuthenticationCommandValidator : AbstractValidator<ResendOtpAuthenticationCommand>
{
    public ResendOtpAuthenticationCommandValidator()
    {
        RuleFor(c => c.ClientId)
            .NotEmpty();
    }
}