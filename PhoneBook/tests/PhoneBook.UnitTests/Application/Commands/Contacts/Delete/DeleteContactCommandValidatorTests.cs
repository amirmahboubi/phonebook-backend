using PhoneBook.Application.Commands.Contacts.Delete;
using PhoneBook.Core.Contracts.Contacts.Commands;

namespace PhoneBook.UnitTests.Application.Commands.Contacts.Delete;

public sealed class DeleteContactCommandValidatorTests
{
    private readonly DeleteContactCommandValidator _validator = new();

    [Fact]
    public void Validate_WithValidCommand_ShouldBeValid()
    {
        // Arrange
        var command =
            new DeleteContactCommand(Guid.NewGuid());

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
        var command =
            new DeleteContactCommand(Guid.Empty);

        // Act
        var result = _validator.Validate(command);

        // Assert
        var error = Assert.Single(result.Errors);

        Assert.Equal(
            nameof(command.ContactId),
            error.PropertyName);

        Assert.Equal(
            "Contact ID is required.",
            error.ErrorMessage);
    }
}
