using Domain.Common;

namespace Application.Users.SendOtp;

public static class SendOtpUserErrors
{
    public const string Prefix = "SendOtpUserErrors";
    
    public static readonly Error DeviceAlreadyExists = Error.Conflict($"{Prefix}.{nameof(DeviceAlreadyExists)}", "The client id already exists.");
    public static readonly Error CodeAlreadySent = Error.Conflict($"{Prefix}.{nameof(CodeAlreadySent)}", "There is a pending OTP session for this phone number.");

}