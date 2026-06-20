using Domain.Entities;
using Domain.ValueObjects;

namespace Application.Common.Interfaces.Repositories;


public interface IUserRepository
{
    Task<bool> ActiveUserExistsByPhoneNumberAsync(PhoneNumber phoneNumber, CancellationToken cancellationToken);
    Task<bool> NotActiveUserExistsByPhoneNumberAsync(PhoneNumber phoneNumber, CancellationToken cancellationToken);
    Task<bool> UserExistsAsync(PhoneNumber phoneNumber, CancellationToken cancellationToken);
    Task<bool> UserExistsAsync(Username username, CancellationToken cancellationToken);
    Task<User?> GetUserByPhoneNumberAsync(PhoneNumber phoneNumber, CancellationToken cancellationToken);
    Task AddAsync(User user, CancellationToken cancellationToken);
}