using Application.Common.Messaging;
using Domain.Common;

namespace Application.Users.Login;


public sealed record LoginUserCommand(string Email, string Password) : ICommand<LoginUserResponse>;