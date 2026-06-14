using Domain.ValueObjects;
using Domain.Errors;
using Domain.Common;

namespace Domain.UnitTests.ValueObjects;

public sealed class PhoneNumberTests
{
    [Fact]
    public void Create_WithNull_ShouldReturnNullError()
    {
        // Arrange
        string phoneNumber = null!;

        // Act
        var result = PhoneNumber.Create(phoneNumber);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(Error.NullValue, result.Error);
    }

    [Fact]
    public void Create_WithInvalidLength_ShouldReturnCharacterLengthError()
    {
        // Arrange
        var phoneNumber = "09123";

        // Act
        var result = PhoneNumber.Create(phoneNumber);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(PhoneNumberErrors.CharacterLengthError, result.Error);
    }

    [Fact]
    public void Create_WithInvalidPattern_ShouldReturnComplexityError()
    {
        // Arrange
        var phoneNumber = "08123456789"; // starts with 08 invalid

        // Act
        var result = PhoneNumber.Create(phoneNumber);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(PhoneNumberErrors.ComplexityError, result.Error);
    }

    [Fact]
    public void Create_WithValidPhoneNumber_ShouldReturnSuccess()
    {
        // Arrange
        var phoneNumber = "09123456789";

        // Act
        var result = PhoneNumber.Create(phoneNumber);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("09123456789", result.Value.Value);
    }

    [Fact]
    public void TwoValidPhoneNumbers_WithSameValue_ShouldBeEqual()
    {
        // Arrange
        var p1 = PhoneNumber.Create("09123456789").Value;
        var p2 = PhoneNumber.Create("09123456789").Value;

        // Act & Assert
        Assert.Equal(p1, p2);
    }
}