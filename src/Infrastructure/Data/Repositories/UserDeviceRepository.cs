using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public sealed class UserDeviceRepository(ApplicationDbContext dbContext) : IUserDeviceRepository
{
    public async Task AddAsync(UserDevice userDevice, CancellationToken cancellationToken)
    {
        await dbContext.UsersDevices.AddAsync(userDevice, cancellationToken);
    }

    public async Task<UserDevice?> GetByClientIdAsync(Guid clientId, CancellationToken cancellationToken)
    {
        return await dbContext.UsersDevices
            .Where(ud => ud.ClientId == clientId)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<UserDevice?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await dbContext.UsersDevices
            .FirstOrDefaultAsync(ud => ud.Id == id, cancellationToken);
    }

    public async Task<Guid?> GetIdByClientIdAsync(Guid clientId, CancellationToken cancellationToken)
    {
        return await dbContext.UsersDevices
            .Where(ud => ud.ClientId == clientId)
            .Select(ud => (Guid?)ud.Id)
            .FirstOrDefaultAsync(cancellationToken);
    }
}