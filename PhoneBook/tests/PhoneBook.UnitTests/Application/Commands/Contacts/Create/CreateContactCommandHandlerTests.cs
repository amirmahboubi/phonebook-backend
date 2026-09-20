using PhoneBook.Core.Contracts.Contacts.Commands;
using PhoneBook.Application.Commands.Contacts.Create;
using PhoneBook.Application.Commands.Common.Exceptions;
using PhoneBook.UnitTests.Application.TestDoubles.Contacts;

namespace PhoneBook.UnitTests.Application.Commands.Contacts.Create;

public sealed class CreateContactCommandHandlerTests
{
    [Fact]
    public void Handle_WithValidCommand_ShouldCreateContact()
    {
        // Arrange
        var repository = new FakeContactRepository();
        var validator = new CreateContactCommandValidator();

        var handler = new CreateContactCommandHandler(
            repository,
            validator);

        var command = new CreateContactCommand(
            "Pouya",
            "Mahboubi",
            "09121234567",
            "Work");

        // Act
        var result = handler.Handle(command);

        // Assert
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal("Pouya", result.FirstName);
        Assert.Equal("Mahboubi", result.LastName);
        Assert.Equal("09121234567", result.PhoneNumber);
        Assert.Equal("Work", result.Tag);

        Assert.Equal(1, repository.AddCallCount);

        var storedContact =
            repository.GetById(result.Id);

        Assert.NotNull(storedContact);
    }

    [Fact]
    public void Handle_ShouldNormalizeInputThroughDomain()
    {
        // Arrange
        var repository = new FakeContactRepository();
        var validator = new CreateContactCommandValidator();

        var handler = new CreateContactCommandHandler(
            repository,
            validator);

        var command = new CreateContactCommand(
            "  Pouya  ",
            "  Mahboubi  ",
            "  09121234567  ",
            "  Work  ");

        // Act
        var result = handler.Handle(command);

        // Assert
        Assert.Equal("Pouya", result.FirstName);
        Assert.Equal("Mahboubi", result.LastName);
        Assert.Equal("09121234567", result.PhoneNumber);
        Assert.Equal("Work", result.Tag);
    }

    [Fact]
    public void Handle_WithInvalidCommand_ShouldThrowValidationException()
    {
        // Arrange
        var repository = new FakeContactRepository();
        var validator = new CreateContactCommandValidator();

        var handler = new CreateContactCommandHandler(
            repository,
            validator);

        var command = new CreateContactCommand(
            null,
            "",
            " ",
            null);

        // Act
        var action = () => handler.Handle(command);

        // Assert
        var exception =
            Assert.Throws<ValidationException>(action);

        Assert.Equal(4, exception.Errors.Count);
        Assert.Equal(0, repository.AddCallCount);
        Assert.Empty(repository.GetAll());
    }
}
