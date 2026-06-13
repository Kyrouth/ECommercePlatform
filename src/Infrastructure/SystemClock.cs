using Application.Common;

namespace Infrastructure;


public sealed class SystemClock : IClockProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}