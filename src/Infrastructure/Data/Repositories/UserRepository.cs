using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public sealed class UserRepository(ApplicationDbContext dbContext) : IUserRepository
{
    public async Task<bool> ActiveUserExistsByPhoneNumberAsync(PhoneNumber phoneNumber, CancellationToken cancellationToken)
    {
        return await dbContext.Users
            .AnyAsync(u =>
                u.State == Domain.Enums.UserState.Active &&
                u.PhoneNumber.Value == phoneNumber.Value,
                cancellationToken
            );
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken)
    {
        await dbContext.Users.AddAsync(user, cancellationToken);
    }

    public async Task<User?> GetUserByPhoneNumberAsync(PhoneNumber phoneNumber, CancellationToken cancellationToken)
    {
        return await dbContext.Users
            .Where(u => u.PhoneNumber.Value == phoneNumber.Value)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> NotActiveUserExistsByPhoneNumberAsync(PhoneNumber phoneNumber, CancellationToken cancellationToken)
    {
        return await dbContext.Users
            .AnyAsync(u =>
                u.State != Domain.Enums.UserState.Active &&
                u.PhoneNumber.Value == phoneNumber.Value,
                cancellationToken
            );
    }

    public async Task<bool> UserExistsAsync(PhoneNumber phoneNumber, CancellationToken cancellationToken)
    {
        return await dbContext.Users.AnyAsync(u => u.PhoneNumber.Value == phoneNumber.Value, cancellationToken);
    }

    public async Task<bool> UserExistsAsync(Username username, CancellationToken cancellationToken)
    {
        return await dbContext.Users.AnyAsync(u => u.Username.Value == username.Value, cancellationToken);
    }
} 