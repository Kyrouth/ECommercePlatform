using FluentValidation;

namespace Application.Users.ResendOtp;

public sealed class ResendOtpUserCommandValidator : AbstractValidator<ResendOtpUserCommand>
{
    public ResendOtpUserCommandValidator()
    {
        RuleFor(c => c.ClientId)
            .NotEmpty();
    }
}