using Application.Common.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public sealed class UserDeviceRepository(ApplicationDbContext dbContext) : IUserDeviceRepository
{
    public async Task<bool> DeviceExistsAsync(Guid deviceId, CancellationToken cancellationToken)
    {
        return await dbContext.UsersDevices.AnyAsync(ud => ud.DeviceId == deviceId, cancellationToken);
    } 
}