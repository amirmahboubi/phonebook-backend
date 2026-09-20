using PhoneBook.Application.Commands.Common.Exceptions;
using PhoneBook.Core.Contracts.Common.Validation;

namespace PhoneBook.UnitTests.Application.Commands.Common.Exceptions;

public sealed class ApplicationExceptionTests
{
    [Fact]
    public void ValidationException_ShouldExposeValidationErrors()
    {
        // Arrange
        var errors = new[]
        {
            new ValidationError(
                "FirstName",
                "First name is required."),

            new ValidationError(
                "Tag",
                "Tag is required.")
        };

        // Act
        var exception = new ValidationException(errors);

        // Assert
        Assert.Equal(2, exception.Errors.Count);
        Assert.Contains(
            exception.Errors,
            error => error.PropertyName == "FirstName");

        Assert.Contains(
            exception.Errors,
            error => error.PropertyName == "Tag");
    }

    [Fact]
    public void ContactNotFoundException_ShouldExposeContactId()
    {
        // Arrange
        var contactId = Guid.NewGuid();

        // Act
        var exception =
            new ContactNotFoundException(contactId);

        // Assert
        Assert.Equal(
            contactId,
            exception.ContactId);

        Assert.Contains(
            contactId.ToString(),
            exception.Message);
    }
}