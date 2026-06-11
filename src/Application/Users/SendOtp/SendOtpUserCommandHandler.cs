using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Application.Common.Messaging;
using Domain.Common;
using Domain.Entities;
using Microsoft.VisualBasic;

namespace Application.Users.SendOtp;

public sealed class SendOtpUserCommandHandler(IUserDeviceRepository userDeviceRepository, IUnitOfWork unitOfWork) : ICommandHandler<SendOtpUserCommand, SendOtpUserResponse>
{

    public async Task<Result<SendOtpUserResponse>> Handle(SendOtpUserCommand command, CancellationToken cancellationToken)
    {
        // var errors = new List<Error>();
        // var deviceExists = await userDeviceRepository.DeviceExistsAsync(command.DeviceId, cancellationToken);

        // if (deviceExists)
        //     errors.Add(SendOtpUserErrors.DeviceAlreadyExists);

        // var device = UserDevice.Create(
        //     deviceId: command.DeviceId,
        //     deviceName: command.DeviceName,
        //     userAgent: command.UserAgent,
        //     ipAddress: command.IpAddress
        // );
        var result = new SendOtpUserResponse("JWT Token From Handler", "Refresh token from handler");
        await Task.CompletedTask;
        return Result.Success(result);
    }
}