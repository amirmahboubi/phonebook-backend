using PhoneBook.Core.Domain.Contacts;
using PhoneBook.Core.Contracts.Contacts.Queries;
using PhoneBook.Core.Contracts.Common.Validation;
using PhoneBook.Application.Queries.Contacts.GetByTag;
using PhoneBook.UnitTests.Application.TestDoubles.Contacts;

namespace PhoneBook.UnitTests.Application.Queries.Contacts.GetByTag;

public sealed class GetContactsByTagQueryHandlerTests
{
    [Fact]
    public void Handle_WithMatchingTag_ShouldReturnMatchingContacts()
    {
        // Arrange
        var repository = new FakeContactRepository();

        var workContact = Contact.Create(
            "Pouya",
            "Mahboubi",
            "09121234567",
            "Work");

        var secondWorkContact = Contact.Create(
            "Sara",
            "Mohammadi",
            "09351234567",
            "Work");

        var familyContact = Contact.Create(
            "Ali",
            "Ahmadi",
            "09129876543",
            "Family");

        repository.AddExisting(workContact);
        repository.AddExisting(secondWorkContact);
        repository.AddExisting(familyContact);

        var validator =
            new GetContactsByTagQueryValidator();

        var handler =
            new GetContactsByTagQueryHandler(
                repository,
                validator);

        var query =
            new GetContactsByTagQuery("Work");

        // Act
        var result = handler.Handle(query);

        // Assert
        Assert.Equal(2, result.Count);

        Assert.Contains(
            result,
            contact => contact.Id == workContact.Id);

        Assert.Contains(
            result,
            contact => contact.Id == secondWorkContact.Id);

        Assert.DoesNotContain(
            result,
            contact => contact.Id == familyContact.Id);
    }

    [Fact]
    public void Handle_WhenNoContactMatches_ShouldReturnEmptyCollection()
    {
        // Arrange
        var repository = new FakeContactRepository();

        repository.AddExisting(
            Contact.Create(
                "Pouya",
                "Mahboubi",
                "09121234567",
                "Work"));

        var validator =
            new GetContactsByTagQueryValidator();

        var handler =
            new GetContactsByTagQueryHandler(
                repository,
                validator);

        // Act
        var result =
            handler.Handle(
                new GetContactsByTagQuery("Family"));

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void Handle_ShouldMapDomainContactsToDtos()
    {
        // Arrange
        var repository = new FakeContactRepository();

        var contact = Contact.Create(
            "Pouya",
            "Mahboubi",
            "09121234567",
            "Work");

        repository.AddExisting(contact);

        var validator =
            new GetContactsByTagQueryValidator();

        var handler =
            new GetContactsByTagQueryHandler(
                repository,
                validator);

        // Act
        var result =
            handler.Handle(
                new GetContactsByTagQuery("Work"));

        // Assert
        var dto = Assert.Single(result);

        Assert.Equal(contact.Id, dto.Id);
        Assert.Equal("Pouya", dto.FirstName);
        Assert.Equal("Mahboubi", dto.LastName);
        Assert.Equal("09121234567", dto.PhoneNumber);
        Assert.Equal("Work", dto.Tag);
    }

    [Fact]
    public void Handle_WithWhitespaceAroundTag_ShouldReturnMatchingContacts()
    {
        // Arrange
        var repository = new FakeContactRepository();

        var contact = Contact.Create(
            "Pouya",
            "Mahboubi",
            "09121234567",
            "Work");

        repository.AddExisting(contact);

        var validator =
            new GetContactsByTagQueryValidator();

        var handler =
            new GetContactsByTagQueryHandler(
                repository,
                validator);

        // Act
        var result =
            handler.Handle(
                new GetContactsByTagQuery("  Work  "));

        // Assert
        var dto = Assert.Single(result);

        Assert.Equal(
            contact.Id,
            dto.Id);
    }

    [Fact]
    public void Handle_WithInvalidQuery_ShouldThrowValidationException()
    {
        // Arrange
        var repository = new FakeContactRepository();

        var validator =
            new GetContactsByTagQueryValidator();

        var handler =
            new GetContactsByTagQueryHandler(
                repository,
                validator);

        // Act
        var action = () =>
            handler.Handle(
                new GetContactsByTagQuery(" "));

        // Assert
        var exception =
            Assert.Throws<ValidationException>(action);

        Assert.Single(exception.Errors);
    }
}
