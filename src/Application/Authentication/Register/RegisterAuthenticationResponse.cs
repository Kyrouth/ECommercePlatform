namespace Application.Authentication.Register;

public sealed record class RegisterAuthenticationResponse(
    string AccessToken,
    string RefreshToken
);