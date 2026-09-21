using PhoneBook.Core.Contracts.Contacts.Queries;
using PhoneBook.Application.Queries.Contacts.GetByTag;

namespace PhoneBook.UnitTests.Application.Queries.Contacts.GetByTag;

public sealed class GetContactsByTagQueryValidatorTests
{
    private readonly GetContactsByTagQueryValidator _validator = new();

    [Fact]
    public void Validate_WithValidTag_ShouldBeValid()
    {
        // Arrange
        var query =
            new GetContactsByTagQuery("Work");

        // Act
        var result = _validator.Validate(query);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
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
        var query =
            new GetContactsByTagQuery(tag);

        // Act
        var result = _validator.Validate(query);

        // Assert
        var error = Assert.Single(result.Errors);

        Assert.Equal(
            nameof(query.Tag),
            error.PropertyName);

        Assert.Equal(
            "Tag is required.",
            error.ErrorMessage);
    }
}
