using PhoneBook.Core.Contracts.Contacts.Queries;

namespace PhoneBook.UnitTests.Application.Queries.Contacts.GetById;

public sealed class GetContactByIdQueryValidatorTests
{
    private readonly GetContactByIdQueryValidator _validator = new();

    [Fact]
    public void Validate_WithValidContactId_ShouldBeValid()
    {
        // Arrange
        var query = new GetContactByIdQuery(
            Guid.NewGuid());

        // Act
        var result = _validator.Validate(query);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Validate_WithEmptyContactId_ShouldReturnError()
    {
        // Arrange
        var query = new GetContactByIdQuery(
            Guid.Empty);

        // Act
        var result = _validator.Validate(query);

        // Assert
        var error = Assert.Single(result.Errors);

        Assert.Equal(
            nameof(query.ContactId),
            error.PropertyName);
    }
}
