namespace Application.Users.Login;

public sealed record LoginUserResponse(string token, string refreshToken);