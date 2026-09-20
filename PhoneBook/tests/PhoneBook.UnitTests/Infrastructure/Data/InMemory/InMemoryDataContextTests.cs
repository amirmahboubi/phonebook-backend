using PhoneBook.Core.Domain.Contacts;
using PhoneBook.Infrastructure.Data.InMemory;

namespace PhoneBook.UnitTests.Infrastructure.Data.InMemory;

public sealed class InMemoryDataContextTests
{
    [Fact]
    public void TryAdd_WithNewContact_ShouldReturnTrue()
    {
        // Arrange
        var context = new InMemoryDataContext();

        var contact = Contact.Create(
            "Pouya",
            "Mahboubi",
            "09121234567",
            "Work");

        // Act
        var result = context.TryAdd(contact);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void TryAdd_WithDuplicateContactId_ShouldReturnFalse()
    {
        // Arrange
        var context = new InMemoryDataContext();

        var contact = Contact.Create(
            "Pouya",
            "Mahboubi",
            "09121234567",
            "Work");

        context.TryAdd(contact);

        // Act
        var result = context.TryAdd(contact);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void TryGet_WithExistingContact_ShouldReturnContact()
    {
        // Arrange
        var context = new InMemoryDataContext();

        var contact = Contact.Create(
            "Pouya",
            "Mahboubi",
            "09121234567",
            "Work");

        context.TryAdd(contact);

        // Act
        var result = context.TryGet(
            contact.Id,
            out var storedContact);

        // Assert
        Assert.True(result);
        Assert.Same(contact, storedContact);
    }

    [Fact]
    public void TryGet_WithUnknownId_ShouldReturnFalse()
    {
        // Arrange
        var context = new InMemoryDataContext();

        // Act
        var result = context.TryGet(
            Guid.NewGuid(),
            out var contact);

        // Assert
        Assert.False(result);
        Assert.Null(contact);
    }

    [Fact]
    public void TryRemove_WithExistingContact_ShouldReturnTrue()
    {
        // Arrange
        var context = new InMemoryDataContext();

        var contact = Contact.Create(
            "Pouya",
            "Mahboubi",
            "09121234567",
            "Work");

        context.TryAdd(contact);

        // Act
        var result = context.TryRemove(
            contact.Id,
            out var removedContact);

        // Assert
        Assert.True(result);
        Assert.Same(contact, removedContact);
        Assert.Empty(context.Contacts);
    }

    [Fact]
    public void TryRemove_WithUnknownId_ShouldReturnFalse()
    {
        // Arrange
        var context = new InMemoryDataContext();

        // Act
        var result = context.TryRemove(
            Guid.NewGuid(),
            out var contact);

        // Assert
        Assert.False(result);
        Assert.Null(contact);
    }

    [Fact]
    public void Contacts_ShouldReturnAllStoredContacts()
    {
        // Arrange
        var context = new InMemoryDataContext();

        var firstContact = Contact.Create(
            "Pouya",
            "Mahboubi",
            "09121234567",
            "Work");

        var secondContact = Contact.Create(
            "Ali",
            "Ahmadi",
            "09129876543",
            "Family");

        context.TryAdd(firstContact);
        context.TryAdd(secondContact);

        // Act
        var result = context.Contacts;

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(firstContact, result);
        Assert.Contains(secondContact, result);
    }

    [Fact]
    public void Contacts_ShouldReturnSnapshot()
    {
        // Arrange
        var context = new InMemoryDataContext();

        var firstContact = Contact.Create(
            "Pouya",
            "Mahboubi",
            "09121234567",
            "Work");

        context.TryAdd(firstContact);

        // Act
        var firstSnapshot = context.Contacts;

        var secondContact = Contact.Create(
            "Ali",
            "Ahmadi",
            "09129876543",
            "Family");

        context.TryAdd(secondContact);

        // Assert
        Assert.Single(firstSnapshot);
        Assert.Equal(2, context.Contacts.Count);
    }
}
