using Domain.Common;
using Domain.Entities;

namespace Domain.Errors;

public static class UserErrors
{
    public static readonly Error FirstNameMaxLengthError = Error.Failure("User.FirstNameMaxLength", "The first name can not be over than 50 characters");
    public static readonly Error LastNameMaxLengthError = Error.Failure($"{nameof(User)}.{nameof(LastNameMaxLengthError)}", "The last name can not be over than 50 characters");

    public static class Username
    {
        public static readonly Error UserNameMinAndMaxLengthError = Error.Failure($"User.{nameof(Username)}.{nameof(UserNameMinAndMaxLengthError)}", $"The username must be at least {ValueObjects.Username.MinLength} characters and can not be over than {ValueObjects.Username.MaxLength} characters.");
        public static readonly Error UserNameComplexityError = Error.Failure($"User.{nameof(Username)}.{nameof(UserNameComplexityError)}", "Username must start with a letter and contain only English letters, numbers, and underscore (_).");

    }

}