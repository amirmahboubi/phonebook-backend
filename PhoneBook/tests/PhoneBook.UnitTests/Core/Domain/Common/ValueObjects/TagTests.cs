namespace PhoneBook.UnitTests.Core.Domain.Common.ValueObjects;

public sealed class TagTests
{
    [Theory]
    [InlineData("Work")]
    [InlineData("Colleague")]
    [InlineData("TraberNet")]
    [InlineData("Business")]
    public void Create_WithValidValue_ShouldCreateTag(string value)
    {
        // Act
        var tag = Tag.Create(value);

        // Assert
        Assert.NotNull(tag);
        Assert.Equal(value, tag.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Create_WithEmptyValue_ShouldThrowDomainException(string? value)
    {
        // Act
        var action = () => Tag.Create(value);

        // Assert
        Assert.Throws<DomainException>(action);
    }

    [Fact]
    public void Create_WithLeadingAndTrailingWhitespace_ShouldTrimValue()
    {
        // Arrange
        const string input = "  Work  ";

        // Act
        var tag = Tag.Create(input);

        // Assert
        Assert.Equal("Work", tag.Value);
    }

    [Fact]
    public void Create_WithSameValue_ShouldBeEqual()
    {
        // Arrange
        var first = Tag.Create("Work");
        var second = Tag.Create("Work");

        // Assert
        Assert.Equal(first, second);
    }

    [Fact]
    public void Create_WithDifferentValue_ShouldNotBeEqual()
    {
        // Arrange
        var first = Tag.Create("Work");
        var second = Tag.Create("Friend");

        // Assert
        Assert.NotEqual(first, second);
    }

    [Fact]
    public void Create_WithDifferentCasing_ShouldBeDifferentValues()
    {
        // Arrange
        var upperCase = Tag.Create("Work");
        var lowerCase = Tag.Create("work");

        // Assert
        Assert.NotEqual(upperCase, lowerCase);
    }
}
