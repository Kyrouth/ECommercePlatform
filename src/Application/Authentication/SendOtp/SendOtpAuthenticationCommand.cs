using Application.Common.Messaging;

namespace Application.Authentication.SendOtp;


public sealed record SendOtpAuthenticationCommand(
    Guid ClientId,
    string PhoneNumber,
    string? UserAgent = null,
    string? IpAddress = null

) : ICommand;