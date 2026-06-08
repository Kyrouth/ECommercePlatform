using System.Text.RegularExpressions;
using Domain.Common;
using Domain.Errors;

namespace Domain.ValueObjects;

public class Username : ValueObject
{
    public const int MaxLength = 15;
    public const int MinLength = 5;
    public const string Pattern = @"^[A-Za-z][A-Za-z0-9_]*$";

    private Username(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Username> Create(string username)
    {
        if(username is null)
        {
            return Error.NullValue;
        }

        if(username.Length < MinLength || username.Length > MaxLength)
        {
            return UserErrors.Username.UserNameMinAndMaxLengthError;
        }

        if (!Regex.IsMatch(username, Pattern))
        {
            return UserErrors.Username.UserNameComplexityError;
        }



        return new Username(username);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}