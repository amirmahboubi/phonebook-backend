using PhoneBook.Core.Domain.Common;
using PhoneBook.Core.Domain.Contacts;

namespace PhoneBook.UnitTests.Core.Domain.Contacts;

public sealed class ContactTests
{
    #region Contact Create Tests
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
    #endregion

    #region Contact Update Tests
    [Fact]
    public void Update_WithValidData_ShouldUpdateContactDetails()
    {
        // Arrange
        var contact = Contact.Create(
            "Amir",
            "Mahboubi",
            "09121234567",
            "Work");

        var originalId = contact.Id;

        // Act
        contact.Update(
            "Ali",
            "Ahmadi",
            "09129876543",
            "Friend");

        // Assert
        Assert.Equal(originalId, contact.Id);
        Assert.Equal("Ali", contact.FirstName);
        Assert.Equal("Ahmadi", contact.LastName);
        Assert.Equal("09129876543", contact.PhoneNumber.Value);
        Assert.Equal("Friend", contact.Tag.Value);
    }

    [Fact]
    public void Update_WithLeadingAndTrailingWhitespace_ShouldTrimValues()
    {
        // Arrange
        var contact = Contact.Create(
            "Amir",
            "Mahboubi",
            "09121234567",
            "Work");

        // Act
        contact.Update(
            "  Ali  ",
            "  Ahmadi  ",
            "  09129876543  ",
            "  Friend  ");

        // Assert
        Assert.Equal("Ali", contact.FirstName);
        Assert.Equal("Ahmadi", contact.LastName);
        Assert.Equal("09129876543", contact.PhoneNumber.Value);
        Assert.Equal("Friend", contact.Tag.Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Update_WithInvalidFirstName_ShouldThrowDomainException(
        string? firstName)
    {
        // Arrange
        var contact = Contact.Create(
            "Amir",
            "Mahboubi",
            "09121234567",
            "Work");

        // Act
        var action = () => contact.Update(
            firstName,
            "Ahmadi",
            "09129876543",
            "Friend");

        // Assert
        Assert.Throws<DomainException>(action);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Update_WithInvalidLastName_ShouldThrowDomainException(
        string? lastName)
    {
        // Arrange
        var contact = Contact.Create(
            "Amir",
            "Mahboubi",
            "09121234567",
            "Work");

        // Act
        var action = () => contact.Update(
            "Ali",
            lastName,
            "09129876543",
            "Friend");

        // Assert
        Assert.Throws<DomainException>(action);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Update_WithInvalidPhoneNumber_ShouldThrowDomainException(
        string? phoneNumber)
    {
        // Arrange
        var contact = Contact.Create(
            "Amir",
            "Mahboubi",
            "09121234567",
            "Work");

        // Act
        var action = () => contact.Update(
            "Ali",
            "Ahmadi",
            phoneNumber,
            "Friend");

        // Assert
        Assert.Throws<DomainException>(action);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Update_WithInvalidTag_ShouldThrowDomainException(
        string? tag)
    {
        // Arrange
        var contact = Contact.Create(
            "Amir",
            "Mahboubi",
            "09121234567",
            "Work");

        // Act
        var action = () => contact.Update(
            "Ali",
            "Ahmadi",
            "09129876543",
            tag);

        // Assert
        Assert.Throws<DomainException>(action);
    }

    [Fact]
    public void Update_ShouldPreserveIdentity()
    {
        // Arrange
        var contact = Contact.Create(
            "Pouya",
            "Mahboubi",
            "09121234567",
            "Work");

        var originalId = contact.Id;

        // Act
        contact.Update(
            "Ali",
            "Ahmadi",
            "09129876543",
            "Friend");

        // Assert
        Assert.Equal(originalId, contact.Id);
    }

    [Fact]
    public void Update_ShouldReplaceAllMutableValues()
    {
        // Arrange
        var contact = Contact.Create(
            "Amir",
            "Mahboubi",
            "09121234567",
            "Work");

        // Act
        contact.Update(
            "Sara",
            "Ahmadi",
            "09351234567",
            "Family");

        // Assert
        Assert.Equal("Sara", contact.FirstName);
        Assert.Equal("Ahmadi", contact.LastName);
        Assert.Equal("09351234567", contact.PhoneNumber.Value);
        Assert.Equal("Family", contact.Tag.Value);
    }
    #endregion
}