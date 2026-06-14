using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authentication;
using Application.Common.Interfaces.Messaging;
using Application.Common.Interfaces.Repositories;
using Application.Common.Messaging;
using Application.Common.Utilities;
using Application.Users.Common;
using Application.Users.SendOtp;
using Domain.Common;
using Domain.Errors;

namespace Application.Users.ResendOtp;

public sealed class ResendOtpUserCommandHandler(
    IUserDeviceRepository userDeviceRepository,
    IOtpHasher otpHasher,
    IClockProvider clockProvider,
    IMessageSender messageSender,
    IUnitOfWork unitOfWork,
    IPhoneVerificationSessionRepository phoneVerificationSessionRepository
) : ICommandHandler<ResendOtpUserCommand>
{
    public async Task<Result> Handle(ResendOtpUserCommand request, CancellationToken cancellationToken)
    {
        var deviceId = await userDeviceRepository.GetIdByClientIdAsync(request.ClientId, cancellationToken);

        if (deviceId is null || deviceId.Value == Guid.Empty)
        {
            return CommonUserErrors.ClientNotFoundError;
        }

        var phoneVerificationSession = await phoneVerificationSessionRepository
            .GetPendingSessionByDeviceIdAsync(deviceId.Value, cancellationToken);

        if (phoneVerificationSession is null)
            return CommonUserErrors.PendingSessionNotFoundError;


        var otp = OtpGenerator.Generate();

        var otpHash = otpHasher.Hash(otp, request.ClientId);

        var result = phoneVerificationSession.Refresh(otpHash, clockProvider.UtcNow);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        if(result.IsFailure)
            return result.Error;

        await messageSender.SendAsync($"Your OTP code is {otp}");

        return Result.Success();
    }
}