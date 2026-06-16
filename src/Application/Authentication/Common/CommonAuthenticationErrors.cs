using Domain.Common;

namespace Application.Authentication.Common;

public static class CommonAuthenticationErrors
{
    public const string Prefix = "CommonAuthenticationErrors";
    public static readonly Error ClientNotFoundError = Error.NotFound($"{Prefix}.{nameof(ClientNotFoundError)}", "The client is not exists.");
    public static readonly Error PendingSessionNotFoundError = Error.NotFound($"{Prefix}.{nameof(PendingSessionNotFoundError)}", "There is not any pending session for this client.");
}