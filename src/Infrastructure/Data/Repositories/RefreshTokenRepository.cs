using Application.Common.Interfaces.Repositories;
using Domain.Entities;

namespace Infrastructure.Data.Repositories;


public sealed class RefreshTokenRepository(ApplicationDbContext dbContext) : IRefreshTokenRepository
{
    public async Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
    {
        await dbContext.RefreshTokens.AddAsync(refreshToken, cancellationToken);
    }
}