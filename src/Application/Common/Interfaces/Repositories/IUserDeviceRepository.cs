namespace Application.Common.Interfaces.Repositories;

public interface IUserDeviceRepository
{
    Task<bool> DeviceExistsAsync(Guid deviceId, CancellationToken cancellationToken);
}