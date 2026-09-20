using PhoneBook.Core.Domain.Contacts;
using PhoneBook.Infrastructure.Data.InMemory.Context;

namespace PhoneBook.UnitTests.Infrastructure.Data.InMemory.Context;

public sealed class InMemoryDataContextTests
{
    #region TryAdd Tests
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
    #endregion

    #region TryGet Tests
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
    #endregion

    #region TryRemove Tests
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
    #endregion

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

    #region Seed Tests
    [Fact]
    public void Seed_ShouldLoadContactsIntoContext()
    {
        // Arrange
        var context = new InMemoryDataContext();

        // Act
        context.Seed();

        // Assert
        Assert.NotEmpty(context.Contacts);
    }

    [Fact]
    public void Seed_ShouldLoadExpectedSeedTags()
    {
        // Arrange
        var context = new InMemoryDataContext();

        // Act
        context.Seed();

        // Assert
        Assert.Contains(
            context.Contacts,
            contact => contact.Tag.Value == "Work");

        Assert.Contains(
            context.Contacts,
            contact => contact.Tag.Value == "Family");
    }

    [Fact]
    public void Seed_ShouldNotAddDuplicateContactsWhenCalledMultipleTimes()
    {
        // Arrange
        var context = new InMemoryDataContext();

        // Act
        context.Seed();
        var firstCount = context.Contacts.Count;

        context.Seed();
        var secondCount = context.Contacts.Count;

        // Assert
        Assert.Equal(firstCount, secondCount);
    }
    #endregion
}
