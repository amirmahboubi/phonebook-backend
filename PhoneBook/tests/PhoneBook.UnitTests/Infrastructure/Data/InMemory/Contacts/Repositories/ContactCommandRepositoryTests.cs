using PhoneBook.Core.Domain.Contacts;
using PhoneBook.Infrastructure.Data.InMemory.Context;
using PhoneBook.Infrastructure.Data.InMemory.Contacts.Repositories;

namespace PhoneBook.UnitTests.Infrastructure.Data.InMemory.Contacts.Repositories;

public sealed class ContactCommandRepositoryTests
{
    #region Add Tests
    [Fact]
    public void Add_ShouldStoreContactInContext()
    {
        // Arrange
        var context = new InMemoryDataContext();
        var repository = new ContactCommandRepository(context);

        var contact = Contact.Create(
            "Pouya",
            "Mahboubi",
            "09121234567",
            "Work");

        // Act
        repository.Add(contact);

        // Assert
        Assert.True(
            context.TryGet(
                contact.Id,
                out var storedContact));

        Assert.Same(
            contact,
            storedContact);
    }

    [Fact]
    public void Add_WithDuplicateContact_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var context = new InMemoryDataContext();
        var repository = new ContactCommandRepository(context);

        var contact = Contact.Create(
            "Pouya",
            "Mahboubi",
            "09121234567",
            "Work");

        repository.Add(contact);

        // Act
        var action = () => repository.Add(contact);

        // Assert
        Assert.Throws<InvalidOperationException>(
            action);
    }
    #endregion

    #region Update Tests
    [Fact]
    public void Update_WithExistingContact_ShouldSucceed()
    {
        // Arrange
        var context = new InMemoryDataContext();
        var repository = new ContactCommandRepository(context);

        var contact = Contact.Create(
            "Pouya",
            "Mahboubi",
            "09121234567",
            "Work");

        repository.Add(contact);

        contact.Update(
            "Ali",
            "Ahmadi",
            "09129876543",
            "Friend");

        // Act
        var action = () => repository.Update(contact);

        // Assert
        Assert.Null(
            Record.Exception(action));
    }

    [Fact]
    public void Update_WithUnknownContact_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var context = new InMemoryDataContext();
        var repository = new ContactCommandRepository(context);

        var contact = Contact.Create(
            "Pouya",
            "Mahboubi",
            "09121234567",
            "Work");

        // Act
        var action = () => repository.Update(contact);

        // Assert
        Assert.Throws<KeyNotFoundException>(
            action);
    }
    #endregion

    #region Delete Tests
    [Fact]
    public void Delete_ShouldRemoveContactFromContext()
    {
        // Arrange
        var context = new InMemoryDataContext();
        var repository = new ContactCommandRepository(context);

        var contact = Contact.Create(
            "Pouya",
            "Mahboubi",
            "09121234567",
            "Work");

        repository.Add(contact);

        // Act
        repository.Delete(contact);

        // Assert
        Assert.False(
            context.TryGet(
                contact.Id,
                out _));
    }

    [Fact]
    public void Delete_WithUnknownContact_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var context = new InMemoryDataContext();
        var repository = new ContactCommandRepository(context);

        var contact = Contact.Create(
            "Pouya",
            "Mahboubi",
            "09121234567",
            "Work");

        // Act
        var action = () => repository.Delete(contact);

        // Assert
        Assert.Throws<KeyNotFoundException>(
            action);
    }
    #endregion
}
