using PhoneBook.Core.Contracts.Contacts.Commands;
using PhoneBook.Application.Commands.Contacts.Validators;

namespace PhoneBook.UnitTests.Application.Commands.Contacts.Validators;

public sealed class CreateContactCommandValidatorTests
{
    private readonly CreateContactCommandValidator _validator = new();

    [Fact]
    public void Validate_WithValidCommand_ShouldBeValid()
    {
        // Arrange
        var command = new CreateContactCommand(
            "Pouya",
            "Mahboubi",
            "09121234567",
            "Work");

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Validate_WithInvalidFirstName_ShouldReturnError(
        string? firstName)
    {
        // Arrange
        var command = new CreateContactCommand(
            firstName,
            "Mahboubi",
            "09121234567",
            "Work");

        // Act
        var result = _validator.Validate(command);

        // Assert
        var error = Assert.Single(
            result.Errors,
            error => error.PropertyName == nameof(command.FirstName));

        Assert.Equal(
            "First name is required.",
            error.ErrorMessage);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Validate_WithInvalidLastName_ShouldReturnError(
        string? lastName)
    {
        // Arrange
        var command = new CreateContactCommand(
            "Pouya",
            lastName,
            "09121234567",
            "Work");

        // Act
        var result = _validator.Validate(command);

        // Assert
        var error = Assert.Single(
            result.Errors,
            error => error.PropertyName == nameof(command.LastName));

        Assert.Equal(
            "Last name is required.",
            error.ErrorMessage);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Validate_WithInvalidPhoneNumber_ShouldReturnError(
        string? phoneNumber)
    {
        // Arrange
        var command = new CreateContactCommand(
            "Pouya",
            "Mahboubi",
            phoneNumber,
            "Work");

        // Act
        var result = _validator.Validate(command);

        // Assert
        var error = Assert.Single(
            result.Errors,
            error => error.PropertyName == nameof(command.PhoneNumber));

        Assert.Equal(
            "Phone number is required.",
            error.ErrorMessage);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Validate_WithInvalidTag_ShouldReturnError(
        string? tag)
    {
        // Arrange
        var command = new CreateContactCommand(
            "Pouya",
            "Mahboubi",
            "09121234567",
            tag);

        // Act
        var result = _validator.Validate(command);

        // Assert
        var error = Assert.Single(
            result.Errors,
            error => error.PropertyName == nameof(command.Tag));

        Assert.Equal(
            "Tag is required.",
            error.ErrorMessage);
    }

    [Fact]
    public void Validate_WithMultipleInvalidFields_ShouldReturnAllErrors()
    {
        // Arrange
        var command = new CreateContactCommand(
            null,
            " ",
            null,
            "");

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal(4, result.Errors.Count);

        Assert.Contains(
            result.Errors,
            error => error.PropertyName == nameof(command.FirstName));

        Assert.Contains(
            result.Errors,
            error => error.PropertyName == nameof(command.LastName));

        Assert.Contains(
            result.Errors,
            error => error.PropertyName == nameof(command.PhoneNumber));

        Assert.Contains(
            result.Errors,
            error => error.PropertyName == nameof(command.Tag));
    }
}
