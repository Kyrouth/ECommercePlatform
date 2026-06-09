using Application.Common.Messaging;
using Domain.Common;

namespace Application.Users.Login;

public sealed class LoginUserCommandHandler : ICommandHandler<LoginUserCommand, LoginUserResponse>
{
    public async Task<Result<LoginUserResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var result = new LoginUserResponse("JWT Token From Handler", "Refresh token from handler");
        await Task.CompletedTask;
        return Result.Success(result);
    }
}