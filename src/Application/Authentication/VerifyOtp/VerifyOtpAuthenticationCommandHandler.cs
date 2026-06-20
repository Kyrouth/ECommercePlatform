using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authentication;
using Application.Common.Interfaces.Repositories;
using Application.Common.Messaging;
using Application.Authentication.Common;
using Domain.Common;
using Domain.Entities;
using Application.Common.Utilities;

namespace Application.Authentication.VerifyOtp;

public sealed class VerifyOtpAuthenticationCommandHandler(
    IUserDeviceRepository userDeviceRepository,
    IPhoneVerificationSessionRepository phoneVerificationSessionRepository,
    IOtpHasher otpHasher,
    IClockProvider clockProvider,
    IUnitOfWork unitOfWork,
    ITokenProvider tokenProvider,
    IUserRepository userRepository,
    IClockProvider clock,
    IRefreshTokenRepository refreshTokenRepository
) : ICommandHandler<VerifyOtpAuthenticationCommand, VerifyOtpAuthenticationCommandResponse>
{
    public async Task<Result<VerifyOtpAuthenticationCommandResponse>> Handle(VerifyOtpAuthenticationCommand request, CancellationToken cancellationToken)
    {
        var device = await userDeviceRepository.GetByClientIdAsync(request.ClientId, cancellationToken);

        if (device is null)
            return CommonAuthenticationErrors.ClientNotFoundError;


        var phoneVerificationSession = await phoneVerificationSessionRepository
            .GetPendingSessionByDeviceIdAsync(device.Id, cancellationToken);

        if (phoneVerificationSession is null)
            return CommonAuthenticationErrors.PendingSessionNotFoundError;

        var otpHash = otpHasher.HashOtp(request.Otp, request.ClientId);

        var result = phoneVerificationSession.Verify(otpHash, clockProvider.UtcNow);

        if (result.IsFailure)
            return result.Error;

        VerifyOtpAuthenticationCommandResponse response;
        var user = await userRepository.GetUserByPhoneNumberAsync(phoneVerificationSession.PhoneNumber, cancellationToken);

        if (user is null)
        {
            response = new VerifyOtpAuthenticationCommandResponse(phoneVerificationSession.Id, result.Value, false);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(response);
        }

        if (!user.IsActive())
            return CommonAuthenticationErrors.UnablePhoneNumberError;


        var tokenForRefresh = tokenProvider.CreateRefreshToken();
        var hashRefreshToken = TokenHasher.HashToken(tokenForRefresh);

        var refreshToken = RefreshToken.Create(Guid.NewGuid(), hashRefreshToken, clock.UtcNow, device.Id, user.Id);

        await refreshTokenRepository.AddAsync(refreshToken, cancellationToken);

        device.SetUser(user.Id);

        var token = tokenProvider.Create(user.Id);

        response = new VerifyOtpAuthenticationCommandResponse(phoneVerificationSession.Id, result.Value, true, token, tokenForRefresh);

        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(response);
    }
}