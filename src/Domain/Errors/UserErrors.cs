using Domain.Common;
using Domain.Entities;

namespace Domain.Errors;

public static class UserErrors
{
    public static readonly Error FirstNameMaxLengthError = Error.Failure("User.FirstNameMaxLength", "The first name can not be over than 50 characters");
    public static readonly Error LastNameMaxLengthError = Error.Failure($"{nameof(User)}.{nameof(LastNameMaxLengthError)}", "The last name can not be over than 50 characters");

    public static class Username
    {
        public static readonly Error MinAndMaxLengthError = Error.Failure($"User.{nameof(Username)}.{nameof(MinAndMaxLengthError)}", $"The username must be at least {ValueObjects.Username.MinLength} characters and can not be over than {ValueObjects.Username.MaxLength} characters.");
        public static readonly Error ComplexityError = Error.Failure($"User.{nameof(Username)}.{nameof(ComplexityError)}", "Username must start with a letter and contain only English letters, numbers, and underscore (_).");
    }

    public static class PhoneNumber
    {
        public static readonly Error CharacterLengthError = Error.Failure($"User.{nameof(PhoneNumber)}.{nameof(CharacterLengthError)}", $"Phone number must be {ValueObjects.PhoneNumber.CharacterLength} characters.");
        public static readonly Error ComplexityError = Error.Failure($"User.{nameof(PhoneNumber)}.{nameof(ComplexityError)}", "Phone number must start with 09 and contain 11 digits.");
    }




}