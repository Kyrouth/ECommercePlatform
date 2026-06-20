using Application.Authentication.Common;
using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Authentication;
using Application.Common.Interfaces.Repositories;
using Application.Common.Messaging;
using Application.Common.Utilities;
using Domain.Common;
using Domain.Constants;
using Domain.Entities;
using Domain.ValueObjects;

namespace Application.Authentication.Register;


public sealed class RegisterAuthenticationCommandHandler(
    IPhoneVerificationSessionRepository phoneVerificationSessionRepository,
    IClockProvider clock,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    ITokenProvider tokenProvider,
    IRefreshTokenRepository refreshTokenRepository,
    IUserDeviceRepository userDeviceRepository
) : ICommandHandler<RegisterAuthenticationCommand, RegisterAuthenticationResponse>
{
    public async Task<Result<RegisterAuthenticationResponse>> Handle(RegisterAuthenticationCommand request, CancellationToken cancellationToken)
    {
        var session = await phoneVerificationSessionRepository
            .GetByIdAsync(request.VerificationTokenId, cancellationToken);

        if (session is null)
            return RegisterAuthenticationErrors.SessionNotExistsError;

        var device = await userDeviceRepository.GetByIdAsync(session.DeviceId, cancellationToken);
        if (device is null)
            return CommonAuthenticationErrors.ClientNotFoundError;

        if (!session.IsValidForCreatingUser(clock.UtcNow))
            return RegisterAuthenticationErrors.InvalidSessionError;


        var userExistsByPhoneNumber = await userRepository.UserExistsAsync(session.PhoneNumber, cancellationToken);

        if (userExistsByPhoneNumber)
            return RegisterAuthenticationErrors.UserAlreadyRegistered;

        var username = Username.Create(request.Username);

        if (username.IsFailure)
            return username.Error;

        var userExistsByUsername = await userRepository.UserExistsAsync(username.Value, cancellationToken);

        if (userExistsByUsername)
            return RegisterAuthenticationErrors.UsernameAlreadyExists;

        var user = User.Create(Guid.NewGuid(), request.FirstName, request.LastName, username.Value, session.PhoneNumber, SystemRoles.CustomerId);

        if (user.IsFailure)
            return user.Error;

        await userRepository.AddAsync(user.Value, cancellationToken);

        var tokenForRefresh = tokenProvider.CreateRefreshToken();
        var hashRefreshToken = TokenHasher.HashToken(tokenForRefresh);

        var refreshToken = RefreshToken.Create(Guid.NewGuid(), hashRefreshToken, clock.UtcNow, device.Id, user.Value.Id);

        await refreshTokenRepository.AddAsync(refreshToken, cancellationToken);

        device.SetUser(user.Value.Id);

        var token = tokenProvider.Create(user.Value.Id);

        var response = new RegisterAuthenticationResponse(token, tokenForRefresh);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(response);
    }
}