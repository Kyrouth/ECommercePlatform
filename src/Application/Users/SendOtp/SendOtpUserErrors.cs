using Domain.Common;

namespace Application.Users.SendOtp;

public static class SendOtpUserErrors
{
    public static readonly Error DeviceAlreadyExists = Error.Failure("SendOtpUserErrors.DeviceAlreadyExists", "The device id already exists");

}