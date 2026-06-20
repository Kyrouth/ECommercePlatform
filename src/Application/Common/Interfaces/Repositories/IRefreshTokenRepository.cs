using Domain.Entities;

namespace Application.Common.Interfaces.Repositories;


public interface IRefreshTokenRepository
{
    Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken);
}