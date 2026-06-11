using Domain.Errors;
using Domain.ValueObjects;
using FluentValidation;

namespace Application.Users.SendOtp;

public sealed class SendOtpUserCommandValidator : AbstractValidator<SendOtpUserCommand>
{
    public SendOtpUserCommandValidator()
    {
        RuleFor(c => c.DeviceId)
            .NotEmpty();

        RuleFor(c => c.PhoneNumber)
            .NotEmpty()
            .Length(PhoneNumber.CharacterLength)
            .Matches(PhoneNumber.Pattern);
    }
}