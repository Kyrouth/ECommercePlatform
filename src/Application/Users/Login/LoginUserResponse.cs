namespace Application.Users.Login;

public record LoginUserResponse(string jwt, string refreshToken);