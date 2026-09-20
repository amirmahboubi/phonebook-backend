using PhoneBook.Core.Contracts.Common.Validation;

namespace PhoneBook.UnitTests.Core.Contracts.Common.Validation;

public sealed class ValidationResultTests
{
    [Fact]
    public void Success_ShouldBeValid()
    {
        // Act
        var result = ValidationResult.Success;

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void ResultWithErrors_ShouldNotBeValid()
    {
        // Arrange
        var errors = new[]
        {
            new ValidationError(
                "FirstName",
                "First name is required.")
        };

        // Act
        var result = new ValidationResult(errors);

        // Assert
        Assert.False(result.IsValid);
        Assert.Single(result.Errors);
    }

    [Fact]
    public void ValidationError_ShouldPreservePropertyNameAndMessage()
    {
        // Arrange
        var error = new ValidationError(
            "Tag",
            "Tag is required.");

        // Assert
        Assert.Equal("Tag", error.PropertyName);
        Assert.Equal("Tag is required.", error.ErrorMessage);
    }
}