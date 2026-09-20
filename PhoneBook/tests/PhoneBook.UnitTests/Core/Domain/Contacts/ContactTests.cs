using PhoneBook.Core.Domain.Common;
using PhoneBook.Core.Domain.Contact;

namespace PhoneBook.UnitTests.Core.Domain.Contacts;

public sealed class ContactTests
{
    [Fact]
    public void Create_WithValidData_ShouldCreateContact()
    {
        // Arrange
        const string firstName = "Amir";
        const string lastName = "Mahboubi";
        const string phoneNumber = "09121234567";
        const string tag = "Work";

        // Act
        var contact = Contact.Create(
            firstName,
            lastName,
            phoneNumber,
            tag);

        // Assert
        Assert.NotEqual(Guid.Empty, contact.Id);
        Assert.Equal(firstName, contact.FirstName);
        Assert.Equal(lastName, contact.LastName);
        Assert.Equal(phoneNumber, contact.PhoneNumber.Value);
        Assert.Equal(tag, contact.Tag.Value);
    }

    [Fact]
    public void Create_ShouldGenerateUniqueIdsForDifferentContacts()
    {
        // Act
        var firstContact = Contact.Create(
            "Amir",
            "Mahboubi",
            "09121234567",
            "Work");

        var secondContact = Contact.Create(
            "Ali",
            "Ahmadi",
            "09129876543",
            "Friend");

        // Assert
        Assert.NotEqual(firstContact.Id, secondContact.Id);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Create_WithInvalidFirstName_ShouldThrowDomainException(
        string? firstName)
    {
        // Act
        var action = () => Contact.Create(
            firstName,
            "Mahboubi",
            "09121234567",
            "Work");

        // Assert
        Assert.Throws<DomainException>(action);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Create_WithInvalidLastName_ShouldThrowDomainException(
        string? lastName)
    {
        // Act
        var action = () => Contact.Create(
            "Amir",
            lastName,
            "09121234567",
            "Work");

        // Assert
        Assert.Throws<DomainException>(action);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Create_WithInvalidPhoneNumber_ShouldThrowDomainException(
        string? phoneNumber)
    {
        // Act
        var action = () => Contact.Create(
            "Amir",
            "Mahboubi",
            phoneNumber,
            "Work");

        // Assert
        Assert.Throws<DomainException>(action);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Create_WithInvalidTag_ShouldThrowDomainException(
        string? tag)
    {
        // Act
        var action = () => Contact.Create(
            "Amir",
            "Mahboubi",
            "09121234567",
            tag);

        // Assert
        Assert.Throws<DomainException>(action);
    }

    [Fact]
    public void Create_WithLeadingAndTrailingWhitespace_ShouldTrimValues()
    {
        // Act
        var contact = Contact.Create(
            "  Amir  ",
            "  Mahboubi  ",
            "  09121234567  ",
            "  Work  ");

        // Assert
        Assert.Equal("Amir", contact.FirstName);
        Assert.Equal("Mahboubi", contact.LastName);
        Assert.Equal("09121234567", contact.PhoneNumber.Value);
        Assert.Equal("Work", contact.Tag.Value);
    }

    [Fact]
    public void Create_ShouldPreservePhoneNumberFormatting()
    {
        // Arrange
        const string phoneNumber = "+98 912 123 4567";

        // Act
        var contact = Contact.Create(
            "Amir",
            "Mahboubi",
            phoneNumber,
            "Work");

        // Assert
        Assert.Equal(phoneNumber, contact.PhoneNumber.Value);
    }
}