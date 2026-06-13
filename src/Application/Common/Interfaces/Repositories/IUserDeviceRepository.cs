namespace Application.Common.Interfaces.Repositories;

public interface IUserDeviceRepository
{
    Task<Guid?> GetIdByClientIdAsync(Guid clientId, CancellationToken cancellationToken);
}