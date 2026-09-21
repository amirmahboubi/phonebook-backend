using PhoneBook.Core.Domain.Contacts;
using PhoneBook.Core.Contracts.Contacts.Queries;
using PhoneBook.UnitTests.Application.TestDoubles.Contacts;

namespace PhoneBook.UnitTests.Application.Queries.Contacts.GetAll;

public sealed class GetAllContactsQueryHandlerTests
{
    [Fact]
    public void Handle_WhenContactsExist_ShouldReturnAllContacts()
    {
        // Arrange
        var repository = new FakeContactRepository();

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

        repository.AddExisting(firstContact);
        repository.AddExisting(secondContact);

        var handler = new GetAllContactsQueryHandler(
            repository);

        // Act
        var result = handler.Handle(
            new GetAllContactsQuery());

        // Assert
        Assert.Equal(2, result.Count);

        Assert.Contains(
            result,
            contact => contact.Id == firstContact.Id);

        Assert.Contains(
            result,
            contact => contact.Id == secondContact.Id);
    }

    [Fact]
    public void Handle_WhenNoContactsExist_ShouldReturnEmptyCollection()
    {
        // Arrange
        var repository = new FakeContactRepository();

        var handler = new GetAllContactsQueryHandler(
            repository);

        // Act
        var result = handler.Handle(
            new GetAllContactsQuery());

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void Handle_ShouldMapContactsToDtos()
    {
        // Arrange
        var repository = new FakeContactRepository();

        var contact = Contact.Create(
            "Pouya",
            "Mahboubi",
            "09121234567",
            "Work");

        repository.AddExisting(contact);

        var handler = new GetAllContactsQueryHandler(
            repository);

        // Act
        var result = handler.Handle(
            new GetAllContactsQuery());

        // Assert
        var dto = Assert.Single(result);

        Assert.Equal(contact.Id, dto.Id);
        Assert.Equal(contact.FirstName, dto.FirstName);
        Assert.Equal(contact.LastName, dto.LastName);
        Assert.Equal(contact.PhoneNumber.Value, dto.PhoneNumber);
        Assert.Equal(contact.Tag.Value, dto.Tag);
    }
}