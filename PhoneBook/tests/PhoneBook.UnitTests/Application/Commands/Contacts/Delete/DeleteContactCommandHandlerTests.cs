using PhoneBook.Core.Domain.Contacts;
using PhoneBook.Core.Contracts.Common.Validation;
using PhoneBook.Core.Contracts.Contacts.Commands;
using PhoneBook.Application.Commands.Contacts.Delete;
using PhoneBook.Application.Commands.Common.Exceptions;
using PhoneBook.UnitTests.Application.TestDoubles.Contacts;

namespace PhoneBook.UnitTests.Application.Commands.Contacts.Delete;

public sealed class DeleteContactCommandHandlerTests
{
    [Fact]
    public void Handle_WithExistingContact_ShouldDeleteContact()
    {
        // Arrange
        var repository = new FakeContactRepository();
        var validator = new DeleteContactCommandValidator();

        var contact = Contact.Create(
            "Pouya",
            "Mahboubi",
            "09121234567",
            "Work");

        repository.AddExisting(contact);

        var handler = new DeleteContactCommandHandler(
            repository,
            repository,
            validator);

        var command =
            new DeleteContactCommand(contact.Id);

        // Act
        handler.Handle(command);

        // Assert
        Assert.Null(
            repository.GetById(contact.Id));

        Assert.Equal(
            1,
            repository.DeleteCallCount);
    }

    [Fact]
    public void Handle_WithNonExistingContact_ShouldThrowContactNotFoundException()
    {
        // Arrange
        var repository = new FakeContactRepository();
        var validator = new DeleteContactCommandValidator();

        var handler = new DeleteContactCommandHandler(
            repository,
            repository,
            validator);

        var contactId = Guid.NewGuid();

        var command =
            new DeleteContactCommand(contactId);

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
            repository.DeleteCallCount);
    }

    [Fact]
    public void Handle_WithInvalidCommand_ShouldThrowValidationException()
    {
        // Arrange
        var repository = new FakeContactRepository();
        var validator = new DeleteContactCommandValidator();

        var handler = new DeleteContactCommandHandler(
            repository,
            repository,
            validator);

        var command =
            new DeleteContactCommand(Guid.Empty);

        // Act
        var action = () => handler.Handle(command);

        // Assert
        Assert.Throws<ValidationException>(action);

        Assert.Equal(
            0,
            repository.DeleteCallCount);
    }
}