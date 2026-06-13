using Application.Common.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public sealed class UserDeviceRepository(ApplicationDbContext dbContext) : IUserDeviceRepository
{
    public async Task<Guid?> GetIdByClientIdAsync(Guid clientId, CancellationToken cancellationToken)
    {
        return await dbContext.UsersDevices
            .Where(ud => ud.ClientId == clientId)
            .Select(ud => (Guid?)ud.Id)
            .FirstOrDefaultAsync(cancellationToken);
    }
}