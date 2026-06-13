
using Application.Common.Messaging;

namespace Application.Users.VerifyOtp;



public sealed record VerifyOtpUserCommand(
    Guid ClientId,
    string Otp
) : ICommand<VerifyOtpUserCommandResponse>;