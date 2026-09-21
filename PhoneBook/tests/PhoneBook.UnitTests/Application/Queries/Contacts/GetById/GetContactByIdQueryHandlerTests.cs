using PhoneBook.Core.Domain.Contacts;
using PhoneBook.Core.Contracts.Contacts.Queries;
using PhoneBook.Core.Contracts.Common.Validation;
using PhoneBook.Application.Queries.Contacts.GetById;
using PhoneBook.UnitTests.Application.TestDoubles.Contacts;

namespace PhoneBook.UnitTests.Application.Queries.Contacts.GetById;

public sealed class GetContactByIdQueryHandlerTests
{
    [Fact]
    public void Handle_WithExistingContact_ShouldReturnContact()
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
            new GetContactByIdQueryValidator();

        var handler =
            new GetContactByIdQueryHandler(
                repository,
                validator);

        // Act
        var result = handler.Handle(new GetContactByIdQuery(contact.Id));

        // Assert
        Assert.NotNull(result);
        Assert.Equal(contact.Id, result.Id);
        Assert.Equal(
            contact.FirstName,
            result.FirstName);
        Assert.Equal(
            contact.LastName,
            result.LastName);
        Assert.Equal(
            contact.PhoneNumber.Value,
            result.PhoneNumber);
        Assert.Equal(
            contact.Tag.Value,
            result.Tag);
    }

    [Fact]
    public void Handle_WithNonExistingContact_ShouldReturnNull()
    {
        // Arrange
        var repository = new FakeContactRepository();

        var validator =
            new GetContactByIdQueryValidator();

        var handler =
            new GetContactByIdQueryHandler(
                repository,
                validator);

        // Act
        var result = handler.Handle(
            new GetContactByIdQuery(Guid.NewGuid()));

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void Handle_WithInvalidQuery_ShouldThrowValidationException()
    {
        // Arrange
        var repository = new FakeContactRepository();

        var validator =
            new GetContactByIdQueryValidator();

        var handler =
            new GetContactByIdQueryHandler(
                repository,
                validator);

        // Act
        var action = () =>
            handler.Handle(
                new GetContactByIdQuery(Guid.Empty));

        // Assert
        var exception =
            Assert.Throws<ValidationException>(action);

        var error = Assert.Single(
            exception.Errors);

        Assert.Equal(
            nameof(GetContactByIdQuery.ContactId),
            error.PropertyName);
    }
}
