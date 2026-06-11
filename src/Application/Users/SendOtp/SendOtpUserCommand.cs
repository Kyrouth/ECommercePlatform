using Application.Common.Messaging;

namespace Application.Users.SendOtp;


public sealed record SendOtpUserCommand(
    Guid DeviceId,
    string PhoneNumber,
    string? DeviceName = null,
    string? UserAgent = null,
    string? IpAddress = null

) : ICommand<SendOtpUserResponse>;