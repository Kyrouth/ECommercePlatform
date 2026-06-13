using Domain.Common;

namespace Application.Users.Common;

public static class CommonUserErrors
{
    public const string Prefix = "CommonUserErrors";
    public static readonly Error ClientNotFoundError = Error.NotFound($"{Prefix}.{nameof(ClientNotFoundError)}", "The client is not exists.");
    public static readonly Error PendingSessionNotFoundError = Error.NotFound($"{Prefix}.{nameof(PendingSessionNotFoundError)}", "There is not any pending session for this client.");
}