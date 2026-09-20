using PhoneBook.Application.Commands.Contacts.Update;
using PhoneBook.Core.Contracts.Contacts.Commands;

namespace PhoneBook.UnitTests.Application.Commands.Contacts.Update;

public sealed class UpdateContactCommandValidatorTests
{
    private readonly UpdateContactCommandValidator _validator = new();

    [Fact]
    public void Validate_WithValidCommand_ShouldBeValid()
    {
        // Arrange
        var command = new UpdateContactCommand(
            Guid.NewGuid(),
            "Ali",
            "Ahmadi",
            "09129876543",
            "Friend");

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Validate_WithEmptyContactId_ShouldReturnError()
    {
        // Arrange
        var command = new UpdateContactCommand(
            Guid.Empty,
            "Ali",
            "Ahmadi",
            "09129876543",
            "Friend");

        // Act
        var result = _validator.Validate(command);

        // Assert
        var error = Assert.Single(
            result.Errors,
            error => error.PropertyName == nameof(command.ContactId));

        Assert.Equal(
            "Contact ID is required.",
            error.ErrorMessage);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Validate_WithInvalidFirstName_ShouldReturnError(
        string? firstName)
    {
        var command = new UpdateContactCommand(
            Guid.NewGuid(),
            firstName,
            "Ahmadi",
            "09129876543",
            "Friend");

        var result = _validator.Validate(command);

        Assert.Contains(
            result.Errors,
            error => error.PropertyName == nameof(command.FirstName));
    }

    [Fact]
    public void Validate_WithMultipleInvalidFields_ShouldReturnAllErrors()
    {
        // Arrange
        var command = new UpdateContactCommand(
            Guid.Empty,
            null,
            "",
            " ",
            null);

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal(5, result.Errors.Count);
    }
}