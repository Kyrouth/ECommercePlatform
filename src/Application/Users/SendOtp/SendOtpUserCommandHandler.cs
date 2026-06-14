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

namespace Application.Users.SendOtp;

public sealed class SendOtpUserCommandHandler(
    IUserDeviceRepository userDeviceRepository,
    IPhoneVerificationSessionRepository phoneVerificationSessionRepository,
    IUnitOfWork unitOfWork,
    IOtpHasher otpHasher,
    IClockProvider clock,
    IMessageSender messageSender
) : ICommandHandler<SendOtpUserCommand>
{

    public async Task<Result> Handle(SendOtpUserCommand request, CancellationToken cancellationToken)
    {

        var phoneNumber = PhoneNumber.Create(request.PhoneNumber);

        if (phoneNumber.IsFailure)
            return phoneNumber.Error;


        var isAPendingSessionExistsForThisPhoneNumber = await phoneVerificationSessionRepository.PendingOtpSessionExistsAsync(phoneNumber.Value, cancellationToken);

        if (isAPendingSessionExistsForThisPhoneNumber)
            return SendOtpUserErrors.CodeAlreadySentError;


        var existsDevice = await userDeviceRepository.GetIdByClientIdAsync(request.ClientId, cancellationToken);

        var otp = OtpGenerator.Generate();
        string otpHash;
        Guid deviceId;

        if (existsDevice is not null && existsDevice.Value != Guid.Empty)
        {
            var anyPendingSession = await phoneVerificationSessionRepository
                .AnyPendingSessionByDeviceIdAsync(existsDevice.Value, cancellationToken);

            if (anyPendingSession)
                return SendOtpUserErrors.SessionAlreadyExistsForClientError;
            
            otpHash = otpHasher.Hash(otp, request.ClientId);
            deviceId = existsDevice.Value;
        }
        else
        {
            var device = UserDevice.Create(
                clientId: request.ClientId,
                userAgent: request.UserAgent,
                ipAddress: request.IpAddress
            );

            await userDeviceRepository.AddAsync(device, cancellationToken);

            otpHash = otpHasher.Hash(otp, device.ClientId);
            deviceId = device.Id;
        }


        var otpSession = PhoneVerificationSession.Create(phoneNumber.Value, otpHash, clock.UtcNow, deviceId);

        await phoneVerificationSessionRepository.AddAsync(otpSession, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        await messageSender.SendAsync($"Your OTP code is {otp}");

        return Result.Success();
    }
}