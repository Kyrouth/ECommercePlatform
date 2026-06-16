using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authentication;
using Application.Common.Interfaces.Repositories;
using Application.Common.Messaging;
using Application.Authentication.Common;
using Domain.Common;

namespace Application.Authentication.VerifyOtp;

public sealed class VerifyOtpAuthenticationCommandHandler(
    IUserDeviceRepository userDeviceRepository,
    IPhoneVerificationSessionRepository phoneVerificationSessionRepository,
    IOtpHasher otpHasher,
    IClockProvider clockProvider,
    IUnitOfWork unitOfWork
) : ICommandHandler<VerifyOtpAuthenticationCommand, VerifyOtpAuthenticationCommandResponse>
{
    public async Task<Result<VerifyOtpAuthenticationCommandResponse>> Handle(VerifyOtpAuthenticationCommand request, CancellationToken cancellationToken)
    {
        var deviceId = await userDeviceRepository.GetIdByClientIdAsync(request.ClientId, cancellationToken);

        if (deviceId is null || deviceId.Value == Guid.Empty)
            return CommonAuthenticationErrors.ClientNotFoundError;


        var phoneVerificationSession = await phoneVerificationSessionRepository
            .GetPendingSessionByDeviceIdAsync(deviceId.Value, cancellationToken);

        if (phoneVerificationSession is null)
            return CommonAuthenticationErrors.PendingSessionNotFoundError;

        var otpHash = otpHasher.Hash(request.Otp, request.ClientId);

        var result = phoneVerificationSession.Verify(otpHash, clockProvider.UtcNow);

        if(result.IsFailure)
            return result.Error;

        var response = new VerifyOtpAuthenticationCommandResponse(phoneVerificationSession.Id, result.Value, false);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(response);
    }
}