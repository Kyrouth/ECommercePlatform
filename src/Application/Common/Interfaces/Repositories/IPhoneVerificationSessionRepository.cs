using Domain.Entities;
using Domain.ValueObjects;

namespace Application.Common.Interfaces.Repositories;

public interface IPhoneVerificationSessionRepository
{
    Task AddAsync(PhoneVerificationSession instance, CancellationToken cancellationToken);
    Task<bool> PendingOtpSessionExistsAsync(PhoneNumber phoneNumber, CancellationToken cancellationToken);
    Task<PhoneVerificationSession?> GetPendingSessionByDeviceIdAsync(Guid deviceId, CancellationToken cancellationToken);
}