using Domain.Common;

namespace Application.Authentication.Register;

public static class RegisterAuthenticationErrors
{
    public const string Prefix = "RegisterAuthenticationErrors";
    
    public static readonly Error SessionNotExistsError = Error.NotFound($"{Prefix}.{nameof(SessionNotExistsError)}", "The verificationId is not found");
    public static readonly Error InvalidSessionError = Error.Conflict($"{Prefix}.{nameof(InvalidSessionError)}", "The session is not valid");
    public static readonly Error UserAlreadyRegistered = Error.Conflict($"{Prefix}.{nameof(UserAlreadyRegistered)}", "The user is already registered");
    public static readonly Error UsernameAlreadyExists = Error.Conflict($"{Prefix}.{nameof(UsernameAlreadyExists)}", "The username is already exists");
}