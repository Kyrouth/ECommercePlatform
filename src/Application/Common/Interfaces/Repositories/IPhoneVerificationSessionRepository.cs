using Domain.Entities;
using Domain.ValueObjects;

namespace Application.Common.Interfaces.Repositories;

public interface IPhoneVerificationSessionRepository
{
    Task AddAsync(PhoneVerificationSession instance, CancellationToken cancellationToken);
    Task<bool> ActiveOtpSessionExistsAsync(PhoneNumber phoneNumber, CancellationToken cancellationToken);
    Task<PhoneVerificationSession?> GetByIdAsync(Guid phoneVerificationId, CancellationToken cancellationToken);
    Task<PhoneVerificationSession?> GetPendingSessionByDeviceIdAsync(Guid deviceId, CancellationToken cancellationToken);
    Task<bool> AnyPendingSessionByDeviceIdAsync(Guid deviceId, CancellationToken cancellationToken);
}