using Domain.Errors;
using Domain.ValueObjects;
using FluentValidation;

namespace Application.Authentication.SendOtp;

public sealed class SendOtpAuthenticationCommandValidator : AbstractValidator<SendOtpAuthenticationCommand>
{
    public SendOtpAuthenticationCommandValidator()
    {
        RuleFor(c => c.ClientId)
            .NotEmpty();

        RuleFor(c => c.PhoneNumber)
            .NotEmpty()
            .Length(PhoneNumber.CharacterLength)
            .Matches(PhoneNumber.Pattern);
    }
}