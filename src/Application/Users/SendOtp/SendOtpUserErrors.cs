using Domain.Common;

namespace Application.Users.SendOtp;

public static class SendOtpUserErrors
{
    public const string Prefix = "SendOtpUserErrors";
    
    public static readonly Error ActiveSessionExistsError = Error.Conflict($"{Prefix}.{nameof(ActiveSessionExistsError)}", "There is a active OTP session for this phone number.");
    public static readonly Error SessionAlreadyExistsForClientError = Error.Conflict($"{Prefix}.{nameof(SessionAlreadyExistsForClientError)}", "There is a pending OTP session for this client.");

}