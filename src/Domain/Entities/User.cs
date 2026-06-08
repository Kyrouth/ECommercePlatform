using Domain.Common;
using Domain.Enums;
using Domain.Errors;
using Domain.ValueObjects;

namespace Domain.Entities;

public class User : BaseAuditableEntity
{
    private User(
        Guid id,
        string firstName,
        string lastName,
        Username username,
        string hashPassword,
        UserState state,
        Guid roleId
    )
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Username = username;
        HashPassword = hashPassword;
        State = state;
        RoleId = roleId;
    }
    public string FirstName { get; private set; } 
    public string LastName { get; private set; }
    public Username Username { get; private set; }
    public string HashPassword { get; private set; }
    public UserState State { get; private set; }
    public Guid RoleId { get; private set; }


    public static Result<User> Create(
        string firstName,
        string lastName,
        Username username,
        string hashPassword,
        Guid roleId
    )
    {
        if(
            string.IsNullOrWhiteSpace(firstName) ||
            string.IsNullOrWhiteSpace(lastName) ||
            string.IsNullOrWhiteSpace(hashPassword)
        )
        {
            return Result.Failure<User>(Error.NullValue);
        }

        if(firstName.Length > 50)
        {
            return UserErrors.FirstNameMaxLengthError;
        }

        if(lastName.Length > 50)
        {
            return UserErrors.LastNameMaxLengthError;
        }

        
        

        return new User(Guid.NewGuid(), firstName, lastName, username, hashPassword,  UserState.Active, roleId);
    }

    public Result<User> Update(
        string firstName,
        string lastName,
        Username username,
        string hashPassword,
        Guid roleId
    )
    {
        if(
            string.IsNullOrWhiteSpace(firstName) ||
            string.IsNullOrWhiteSpace(lastName) ||
            string.IsNullOrWhiteSpace(hashPassword)
        )
        {
            return Result.Failure<User>(Error.NullValue);
        }

        if(firstName.Length > 50)
        {
            return UserErrors.FirstNameMaxLengthError;
        }

        if(lastName.Length > 50)
        {
            return UserErrors.LastNameMaxLengthError;
        }


        

        return new User(Guid.NewGuid(), firstName, lastName, username, hashPassword,  UserState.Active, roleId);
    }


}