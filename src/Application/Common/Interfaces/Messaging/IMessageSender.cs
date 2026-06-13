namespace Application.Common.Interfaces.Messaging;

public interface IMessageSender
{
    Task SendAsync(string message);
}