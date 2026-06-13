namespace Application.Common;

public interface IClockProvider
{
    public DateTime UtcNow { get; }
}