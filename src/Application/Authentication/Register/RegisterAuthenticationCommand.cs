using Application.Common.Messaging;

namespace Application.Authentication.Register;

public sealed record RegisterAuthenticationCommand(
    Guid VerificationTokenId,
    string FirstName,
    string LastName,
    string Username
) : ICommand<RegisterAuthenticationResponse>;