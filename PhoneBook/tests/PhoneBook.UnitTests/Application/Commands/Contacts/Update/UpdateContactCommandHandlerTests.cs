using PhoneBook.Core.Domain.Contacts;
using PhoneBook.Core.Contracts.Contacts.Commands;
using PhoneBook.Application.Commands.Contacts.Update;
using PhoneBook.Application.Commands.Common.Exceptions;
using PhoneBook.UnitTests.Application.TestDoubles.Contacts;

namespace PhoneBook.UnitTests.Application.Commands.Contacts.Update;

public sealed class UpdateContactCommandHandlerTests
{
    [Fact]
    public void Handle_WithExistingContact_ShouldUpdateContact()
    {
        // Arrange
        var repository = new FakeContactRepository();
        var validator = new UpdateContactCommandValidator();

        var contact = Contact.Create(
            "Pouya",
            "Mahboubi",
            "09121234567",
            "Work");

        repository.AddExisting(contact);

        var handler = new UpdateContactCommandHandler(
            repository,
            repository,
            validator);

        var command = new UpdateContactCommand(
            contact.Id,
            "Ali",
            "Ahmadi",
            "09129876543",
            "Friend");

        // Act
        handler.Handle(command);

        // Assert
        var updated = repository.GetById(contact.Id);

        Assert.NotNull(updated);
        Assert.Equal(contact.Id, updated.Id);
        Assert.Equal("Ali", updated.FirstName);
        Assert.Equal("Ahmadi", updated.LastName);
        Assert.Equal("09129876543", updated.PhoneNumber.Value);
        Assert.Equal("Friend", updated.Tag.Value);

        Assert.Equal(1, repository.UpdateCallCount);
    }

    [Fact]
    public void Handle_WithNonExistingContact_ShouldThrowContactNotFoundException()
    {
        // Arrange
        var repository = new FakeContactRepository();
        var validator = new UpdateContactCommandValidator();

        var handler = new UpdateContactCommandHandler(
            repository,
            repository,
            validator);

        var contactId = Guid.NewGuid();

        var command = new UpdateContactCommand(
            contactId,
            "Ali",
            "Ahmadi",
            "09129876543",
            "Friend");

        // Act
        var action = () => handler.Handle(command);

        // Assert
        var exception =
            Assert.Throws<ContactNotFoundException>(action);

        Assert.Equal(
            contactId,
            exception.ContactId);

        Assert.Equal(
            0,
            repository.UpdateCallCount);
    }

    [Fact]
    public void Handle_WithInvalidCommand_ShouldThrowValidationException()
    {
        // Arrange
        var repository = new FakeContactRepository();
        var validator = new UpdateContactCommandValidator();

        var handler = new UpdateContactCommandHandler(
            repository,
            repository,
            validator);

        var command = new UpdateContactCommand(
            Guid.Empty,
            null,
            "",
            " ",
            null);

        // Act
        var action = () => handler.Handle(command);

        // Assert
        var exception =
            Assert.Throws<ValidationException>(action);

        Assert.Equal(5, exception.Errors.Count);
        Assert.Equal(0, repository.UpdateCallCount);
    }

    [Fact]
    public void Handle_ShouldPreserveContactIdentity()
    {
        // Arrange
        var repository = new FakeContactRepository();
        var validator = new UpdateContactCommandValidator();

        var contact = Contact.Create(
            "Pouya",
            "Mahboubi",
            "09121234567",
            "Work");

        var originalId = contact.Id;

        repository.AddExisting(contact);

        var handler = new UpdateContactCommandHandler(
            repository,
            repository,
            validator);

        var command = new UpdateContactCommand(
            originalId,
            "Sara",
            "Mohammadi",
            "09351234567",
            "Family");

        // Act
        handler.Handle(command);

        // Assert
        Assert.Equal(
            originalId,
            contact.Id);
    }
}
