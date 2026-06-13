using Application.Common;
using Application.Common.Interfaces.Authentication;
using Application.Common.Interfaces.Repositories;
using Application.Common.Messaging;
using Application.Users.Common;
using Application.Users.VerifyOtp;
using Domain.Common;
using MediatR;

namespace Application.Users.SendOtp;

public sealed class VerifyOtpUserCommandHandler(
    IUserDeviceRepository userDeviceRepository,
    IPhoneVerificationSessionRepository phoneVerificationSessionRepository,
    IOtpHasher otpHasher,
    IClockProvider clockProvider
) : ICommandHandler<VerifyOtpUserCommand, VerifyOtpUserCommandResponse>
{
    async Task<Result<VerifyOtpUserCommandResponse>> IRequestHandler<VerifyOtpUserCommand, Result<VerifyOtpUserCommandResponse>>.Handle(VerifyOtpUserCommand request, CancellationToken cancellationToken)
    {
        var deviceId = await userDeviceRepository.GetIdByClientIdAsync(request.ClientId, cancellationToken);

        if (deviceId is null || deviceId.Value == Guid.Empty)
            return CommonUserErrors.ClientNotFoundError;


        var phoneVerificationSession = await phoneVerificationSessionRepository
            .GetPendingSessionByDeviceIdAsync(deviceId.Value, cancellationToken);

        if (phoneVerificationSession is null)
            return CommonUserErrors.PendingSessionNotFoundError;

        var otpHash = otpHasher.Hash(request.Otp, request.ClientId);

        var result = phoneVerificationSession.Verify(otpHash, clockProvider.UtcNow);

        if(result.IsFailure)
            return result.Error;

        var response = new VerifyOtpUserCommandResponse(phoneVerificationSession.Id, result.Value);
        
        return Result.Success(response);
    }
}