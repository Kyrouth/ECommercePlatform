
using Application.Common.Messaging;

namespace Application.Authentication.VerifyOtp;



public sealed record VerifyOtpAuthenticationCommand(
    Guid ClientId,
    string Otp
) : ICommand<VerifyOtpAuthenticationCommandResponse>;