namespace Application.Users.Login;

public sealed record LoginUserResponse(string jwt, string refreshToken);