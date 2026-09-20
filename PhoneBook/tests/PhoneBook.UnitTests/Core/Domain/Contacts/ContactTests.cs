using PhoneBook.Core.Domain.Contact;

namespace PhoneBook.UnitTests.Core.Domain.Contacts;

public class ContactTests
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
        Assert.Equal(phoneNumber, contact.PhoneNumber);
        Assert.Equal(tag, contact.Tag);
    }
}
