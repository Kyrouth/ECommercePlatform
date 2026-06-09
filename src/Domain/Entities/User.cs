using Domain.Common;
using Domain.Enums;
using Domain.Errors;
using Domain.ValueObjects;

namespace Domain.Entities;

public sealed class User : BaseAuditableEntity
{
    private User()
    {
        FirstName = null!;
        LastName = null!;
        Username = null!;
        PhoneNumber = null!;
    }

    private User(
        Guid id,
        string firstName,
        string lastName,
        Username username,
        PhoneNumber phoneNumber,
        UserState state,
        Guid roleId
    )
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Username = username;
        PhoneNumber = phoneNumber;
        State = state;
        RoleId = roleId;
    }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public Username Username { get; private set; }
    public UserState State { get; private set; }
    public Guid RoleId { get; private set; }
    public PhoneNumber PhoneNumber { get; set; }


    public static Result<User> Create(
        string firstName,
        string lastName,
        Username username,
        PhoneNumber phoneNumber,
        Guid roleId
    )
    {
        if (
            string.IsNullOrWhiteSpace(firstName) ||
            string.IsNullOrWhiteSpace(lastName)
        )
        {
            return Result.Failure<User>(Error.NullValue);
        }

        if (firstName.Length > 50)
        {
            return UserErrors.FirstNameMaxLengthError;
        }

        if (lastName.Length > 50)
        {
            return UserErrors.LastNameMaxLengthError;
        }



        return new User(Guid.NewGuid(), firstName, lastName, username, phoneNumber, UserState.Active, roleId);
    }


}