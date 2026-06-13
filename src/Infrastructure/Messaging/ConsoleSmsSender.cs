using Application.Common.Interfaces.Messaging;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Messaging;

public sealed class ConsoleSmsSender(ILogger<ConsoleSmsSender> logger) : IMessageSender
{
    public Task SendAsync(string message)
    {
        logger.LogInformation($"{message}");
        return Task.CompletedTask;
    }
}