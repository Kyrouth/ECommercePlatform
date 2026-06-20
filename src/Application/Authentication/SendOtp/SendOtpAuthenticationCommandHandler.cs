using Application.Authentication.Common;
using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authentication;
using Application.Common.Interfaces.Messaging;
using Application.Common.Interfaces.Repositories;
using Application.Common.Messaging;
using Application.Common.Utilities;
using Domain.Common;
using Domain.Entities;
using Domain.ValueObjects;

namespace Application.Authentication.SendOtp;

public sealed class SendOtpAuthenticationCommandHandler(
    IUserDeviceRepository userDeviceRepository,
    IPhoneVerificationSessionRepository phoneVerificationSessionRepository,
    IUnitOfWork unitOfWork,
    IOtpHasher otpHasher,
    IClockProvider clock,
    IMessageSender messageSender,
    IUserRepository userRepository
) : ICommandHandler<SendOtpAuthenticationCommand>
{

    public async Task<Result> Handle(SendOtpAuthenticationCommand request, CancellationToken cancellationToken)
    {

        var phoneNumber = PhoneNumber.Create(request.PhoneNumber);

        if (phoneNumber.IsFailure)
            return phoneNumber.Error;


        var isAPendingSessionExistsForThisPhoneNumber = await phoneVerificationSessionRepository.ActiveOtpSessionExistsAsync(phoneNumber.Value, cancellationToken);

        if (isAPendingSessionExistsForThisPhoneNumber)
            return SendOtpAuthenticationErrors.ActiveSessionExistsError;
        
        var notActiveUserExistsByPhoneNumber = await userRepository.NotActiveUserExistsByPhoneNumberAsync(phoneNumber.Value, cancellationToken);

        if(notActiveUserExistsByPhoneNumber)
            return CommonAuthenticationErrors.UnablePhoneNumberError;

        var existsDevice = await userDeviceRepository.GetIdByClientIdAsync(request.ClientId, cancellationToken);

        var otp = OtpGenerator.Generate();
        string otpHash;
        Guid deviceId;

        if (existsDevice is not null && existsDevice.Value != Guid.Empty)
        {
            var anyPendingSession = await phoneVerificationSessionRepository
                .AnyPendingSessionByDeviceIdAsync(existsDevice.Value, cancellationToken);

            if (anyPendingSession)
                return SendOtpAuthenticationErrors.SessionAlreadyExistsForClientError;
            
            otpHash = otpHasher.HashOtp(otp, request.ClientId);
            deviceId = existsDevice.Value;
        }
        else
        {
            var device = UserDevice.Create(
                Guid.NewGuid(),
                request.ClientId,
                userAgent: request.UserAgent,
                ipAddress: request.IpAddress
            );

            await userDeviceRepository.AddAsync(device, cancellationToken);

            otpHash = otpHasher.HashOtp(otp, device.ClientId);
            deviceId = device.Id;
        }


        var otpSession = PhoneVerificationSession.Create(Guid.NewGuid(), phoneNumber.Value, otpHash, clock.UtcNow, deviceId);

        await phoneVerificationSessionRepository.AddAsync(otpSession, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        await messageSender.SendAsync($"Your OTP code is {otp}");

        return Result.Success();
    }
}