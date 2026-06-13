using Domain.Common;

namespace Application.Users.SendOtp;

public static class ResendOtpUserErrors
{
    public const string Prefix = "ResendOtpUserErrors";
    public static readonly Error ClientNotFoundError = Error.NotFound($"{Prefix}.{nameof(ClientNotFoundError)}", "The client is not exists.");
    public static readonly Error SessionNotFoundError = Error.NotFound($"{Prefix}.{nameof(SessionNotFoundError)}", "There is not any pending session for this client.");

}