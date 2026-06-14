using Domain.Entities;

namespace Domain.UnitTests.Entities;

public sealed class UserDeviceTests
{
    [Fact]
    public void Create_ShouldSetRequiredPropertiesCorrectly()
    {
        // Arrange
        var clientId = Guid.NewGuid();

        // Act
        var device = UserDevice.Create(clientId);

        // Assert
        Assert.Equal(clientId, device.ClientId);
        Assert.Null(device.UserId);
        Assert.Null(device.UserAgent);
        Assert.Null(device.IpAddress);
        Assert.NotEqual(Guid.Empty, device.Id);
    }

    [Fact]
    public void Create_WithAllParameters_ShouldSetAllProperties()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var userAgent = "Mozilla/5.0";
        var ipAddress = "127.0.0.1";

        // Act
        var device = UserDevice.Create(
            clientId,
            userId,
            userAgent,
            ipAddress
        );

        // Assert
        Assert.Equal(clientId, device.ClientId);
        Assert.Equal(userId, device.UserId);
        Assert.Equal(userAgent, device.UserAgent);
        Assert.Equal(ipAddress, device.IpAddress);
    }

    [Fact]
    public void Create_ShouldAlwaysGenerateNewId()
    {
        // Arrange
        var clientId = Guid.NewGuid();

        // Act
        var device1 = UserDevice.Create(clientId);
        var device2 = UserDevice.Create(clientId);

        // Assert
        Assert.NotEqual(device1.Id, device2.Id);
    }

    [Fact]
    public void Create_ShouldNotAllowEmptyClientIdToBeHandledByEntity()
    {
        // Arrange
        var clientId = Guid.Empty;

        // Act
        var device = UserDevice.Create(clientId);

        // Assert
        Assert.Equal(Guid.Empty, device.ClientId);
    }

    [Fact]
    public void Create_ShouldPreserveUserId_WhenProvided()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        // Act
        var device = UserDevice.Create(clientId, userId: userId);

        // Assert
        Assert.Equal(userId, device.UserId);
    }

    [Fact]
    public void Create_ShouldAllowPartialOptionalData()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var userAgent = "Chrome";

        // Act
        var device = UserDevice.Create(
            clientId,
            userAgent: userAgent
        );

        // Assert
        Assert.Equal(userAgent, device.UserAgent);
        Assert.Null(device.IpAddress);
    }
}