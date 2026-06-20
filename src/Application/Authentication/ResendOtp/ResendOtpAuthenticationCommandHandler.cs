using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authentication;
using Application.Common.Interfaces.Messaging;
using Application.Common.Interfaces.Repositories;
using Application.Common.Messaging;
using Application.Common.Utilities;
using Application.Authentication.Common;
using Domain.Common;

namespace Application.Authentication.ResendOtp;

public sealed class ResendOtpAuthenticationCommandHandler(
    IUserDeviceRepository userDeviceRepository,
    IOtpHasher otpHasher,
    IClockProvider clockProvider,
    IMessageSender messageSender,
    IUnitOfWork unitOfWork,
    IPhoneVerificationSessionRepository phoneVerificationSessionRepository
) : ICommandHandler<ResendOtpAuthenticationCommand>
{
    public async Task<Result> Handle(ResendOtpAuthenticationCommand request, CancellationToken cancellationToken)
    {
        var deviceId = await userDeviceRepository.GetIdByClientIdAsync(request.ClientId, cancellationToken);

        if (deviceId is null || deviceId.Value == Guid.Empty)
        {
            return CommonAuthenticationErrors.ClientNotFoundError;
        }

        var phoneVerificationSession = await phoneVerificationSessionRepository
            .GetPendingSessionByDeviceIdAsync(deviceId.Value, cancellationToken);

        if (phoneVerificationSession is null)
            return CommonAuthenticationErrors.PendingSessionNotFoundError;


        var otp = OtpGenerator.Generate();

        var otpHash = otpHasher.HashOtp(otp, request.ClientId);

        var result = phoneVerificationSession.Refresh(otpHash, clockProvider.UtcNow);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        if(result.IsFailure)
            return result.Error;

        await messageSender.SendAsync($"Your OTP code is {otp}");

        return Result.Success();
    }
}