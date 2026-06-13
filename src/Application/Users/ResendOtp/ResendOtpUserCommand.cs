using Application.Common.Messaging;

namespace Application.Users.ResendOtp;

public sealed record ResendOtpUserCommand(Guid ClientId) : ICommand;