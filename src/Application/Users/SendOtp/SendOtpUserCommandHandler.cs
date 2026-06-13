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
        var deviceExists = await userDeviceRepository.GetIdByClientIdAsync(request.ClientId, cancellationToken);

        if (deviceExists is not null && deviceExists.Value != Guid.Empty)
        {
            return SendOtpUserErrors.DeviceAlreadyExists;
        }

        var phoneNumber = PhoneNumber.Create(request.PhoneNumber);

        if (phoneNumber.IsFailure)
            return phoneNumber.Error;

        var isAPendingSessionExistsForThisPhoneNumber = await phoneVerificationSessionRepository.PendingOtpSessionExistsAsync(phoneNumber.Value, cancellationToken);

        if(isAPendingSessionExistsForThisPhoneNumber)
            return SendOtpUserErrors.CodeAlreadySent;

        var device = UserDevice.Create(
            clientId: request.ClientId,
            userAgent: request.UserAgent,
            ipAddress: request.IpAddress
        );

        var otp = OtpGenerator.Generate();

        var otpHash = otpHasher.Hash(otp, device.ClientId);

        var otpSession = PhoneVerificationSession.Create(phoneNumber.Value, otpHash, clock.UtcNow, device.Id);

        await phoneVerificationSessionRepository.AddAsync(otpSession, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        await messageSender.SendAsync($"Your OTP code is {otp}");

        return Result.Success();
    }
}