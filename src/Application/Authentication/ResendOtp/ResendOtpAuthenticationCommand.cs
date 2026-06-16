using Application.Common.Messaging;

namespace Application.Authentication.ResendOtp;

public sealed record ResendOtpAuthenticationCommand(Guid ClientId) : ICommand;