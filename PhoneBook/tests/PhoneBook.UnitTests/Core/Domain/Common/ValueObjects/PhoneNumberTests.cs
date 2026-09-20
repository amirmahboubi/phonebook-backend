using PhoneBook.Core.Domain.Common;
using PhoneBook.Core.Domain.Common.ValueObjects;

namespace PhoneBook.UnitTests.Core.Domain.Common.ValueObjects;

public sealed class PhoneNumberTests
{
    [Theory]
    [InlineData("09121234567")]
    [InlineData("+989121234567")]
    [InlineData("+98 912 123 4567")]
    [InlineData("021-12345678")]
    public void Create_WithValidValue_ShouldCreatePhoneNumber(string value)
    {
        // Act
        var phoneNumber = PhoneNumber.Create(value);

        // Assert
        Assert.NotNull(phoneNumber);
        Assert.Equal(value, phoneNumber.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Create_WithEmptyValue_ShouldThrowDomainException(string? value)
    {
        // Act
        var action = () => PhoneNumber.Create(value);

        // Assert
        Assert.Throws<DomainException>(action);
    }

    [Fact]
    public void Create_WithLeadingAndTrailingWhitespace_ShouldTrimValue()
    {
        // Arrange
        const string input = "  09121234567  ";

        // Act
        var phoneNumber = PhoneNumber.Create(input);

        // Assert
        Assert.Equal("09121234567", phoneNumber.Value);
    }

    [Fact]
    public void Create_WithSameValue_ShouldBeEqual()
    {
        // Arrange
        var first = PhoneNumber.Create("09121234567");
        var second = PhoneNumber.Create("09121234567");

        // Assert
        Assert.Equal(first, second);
    }

    [Fact]
    public void Create_WithDifferentValue_ShouldNotBeEqual()
    {
        // Arrange
        var first = PhoneNumber.Create("09121234567");
        var second = PhoneNumber.Create("09129876543");

        // Assert
        Assert.NotEqual(first, second);
    }

    [Fact]
    public void Create_ShouldPreservePhoneNumberCharacters()
    {
        // Arrange
        const string input = "+98 912 123 4567";

        // Act
        var phoneNumber = PhoneNumber.Create(input);

        // Assert
        Assert.Equal(input, phoneNumber.Value);
    }

    [Fact]
    public void Create_WithWhitespaceOnlyAroundValue_ShouldNormalizeValue()
    {
        // Arrange
        const string input = "   +98 912 123 4567   ";

        // Act
        var phoneNumber = PhoneNumber.Create(input);

        // Assert
        Assert.Equal("+98 912 123 4567", phoneNumber.Value);
    }
}