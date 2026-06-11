using System.Text.RegularExpressions;
using Domain.Common;
using Domain.Errors;

namespace Domain.ValueObjects;

public sealed class PhoneNumber : ValueObject
{
    public const int CharacterLength = 11;
    public const string Pattern = @"^09\d{9}$";

    private PhoneNumber(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<PhoneNumber> Create(string phoneNumber)
    {
        if(phoneNumber is null)
        {
            return Error.NullValue;
        }

        if(phoneNumber.Length != CharacterLength)
        {
            return PhoneNumberErrors.CharacterLengthError;
        }

        if (!Regex.IsMatch(phoneNumber, Pattern))
        {
            return PhoneNumberErrors.ComplexityError;
        }

        return new PhoneNumber(phoneNumber);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}