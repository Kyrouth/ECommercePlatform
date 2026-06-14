using Application.Common;
using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public sealed class PhoneVerificationSessionRepository(ApplicationDbContext dbContext, IClockProvider clockProvider) : IPhoneVerificationSessionRepository
{
    public async Task AddAsync(PhoneVerificationSession instance, CancellationToken cancellationToken)
    {
        await dbContext.phoneVerificationSessions.AddAsync(instance, cancellationToken);
    }

    public async Task<bool> AnyPendingSessionByDeviceIdAsync(Guid deviceId, CancellationToken cancellationToken)
    {
        return await dbContext.phoneVerificationSessions
            .AnyAsync(pvs => pvs.DeviceId == deviceId && pvs.Status == OtpSessionStatus.Pending, cancellationToken);
    }

    public async Task<PhoneVerificationSession?> GetPendingSessionByDeviceIdAsync(Guid deviceId, CancellationToken cancellationToken)
    {
        return await dbContext.phoneVerificationSessions
            .Where(pvs => pvs.DeviceId == deviceId && pvs.Status == OtpSessionStatus.Pending)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> PendingOtpSessionExistsAsync(PhoneNumber phoneNumber, CancellationToken cancellationToken)
    {
        return await dbContext.phoneVerificationSessions
            .AnyAsync(pvs =>
                pvs.PhoneNumber.Value == phoneNumber.Value &&
                pvs.Status == OtpSessionStatus.Pending &&
                pvs.ExpiresAt > clockProvider.UtcNow,
                cancellationToken
            );
    }
}