using Application.Common.Messaging;

namespace Application.Users.SendOtp;


public sealed record SendOtpUserCommand(
    Guid ClientId,
    string PhoneNumber,
    string? UserAgent = null,
    string? IpAddress = null

) : ICommand;