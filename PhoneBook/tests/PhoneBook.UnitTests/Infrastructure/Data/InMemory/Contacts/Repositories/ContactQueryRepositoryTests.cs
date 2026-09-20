using PhoneBook.Core.Domain.Contacts;
using PhoneBook.Infrastructure.Data.InMemory.Context;
using PhoneBook.Infrastructure.Data.InMemory.Contacts.Repositories;

namespace PhoneBook.UnitTests.Infrastructure.Data.InMemory.Contacts.Repositories;

public sealed class ContactQueryRepositoryTests
{
    #region GetAll Tests
    [Fact]
    public void GetAll_WhenContextIsEmpty_ShouldReturnEmptyCollection()
    {
        // Arrange
        var context = new InMemoryDataContext();
        var repository = new ContactQueryRepository(context);

        // Act
        var result = repository.GetAll();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void GetAll_ShouldReturnAllContacts()
    {
        // Arrange
        var context = new InMemoryDataContext();
        var commandRepository =
            new ContactCommandRepository(context);

        var queryRepository =
            new ContactQueryRepository(context);

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

        commandRepository.Add(firstContact);
        commandRepository.Add(secondContact);

        // Act
        var result = queryRepository.GetAll();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(firstContact, result);
        Assert.Contains(secondContact, result);
    }

    public void GetAll_WhenContextIsSeeded_ShouldReturnSeedContacts()
    {
        // Arrange
        var context = new InMemoryDataContext();
        context.Seed();

        var repository =
            new ContactQueryRepository(context);

        // Act
        var result = repository.GetAll();

        // Assert
        Assert.NotEmpty(result);

        Assert.Contains(
            result,
            contact => contact.Tag.Value == "Work");

        Assert.Contains(
            result,
            contact => contact.Tag.Value == "Family");
    }
    #endregion

    #region GetById Tests
    [Fact]
    public void GetById_WithExistingContact_ShouldReturnContact()
    {
        // Arrange
        var context = new InMemoryDataContext();
        var commandRepository =
            new ContactCommandRepository(context);

        var queryRepository =
            new ContactQueryRepository(context);

        var contact = Contact.Create(
            "Pouya",
            "Mahboubi",
            "09121234567",
            "Work");

        commandRepository.Add(contact);

        // Act
        var result =
            queryRepository.GetById(contact.Id);

        // Assert
        Assert.Same(contact, result);
    }

    [Fact]
    public void GetById_WithUnknownId_ShouldReturnNull()
    {
        // Arrange
        var context = new InMemoryDataContext();
        var repository = new ContactQueryRepository(context);

        // Act
        var result =
            repository.GetById(Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }
    #endregion

    #region GetByTag Tests
    [Fact]
    public void GetByTag_ShouldReturnMatchingContacts()
    {
        // Arrange
        var context = new InMemoryDataContext();
        var commandRepository =
            new ContactCommandRepository(context);

        var queryRepository =
            new ContactQueryRepository(context);

        var workContact1 = Contact.Create(
            "Pouya",
            "Mahboubi",
            "09121234567",
            "Work");

        var workContact2 = Contact.Create(
            "Sara",
            "Mohammadi",
            "09351234567",
            "Work");

        var familyContact = Contact.Create(
            "Ali",
            "Ahmadi",
            "09129876543",
            "Family");

        commandRepository.Add(workContact1);
        commandRepository.Add(workContact2);
        commandRepository.Add(familyContact);

        // Act
        var result =
            queryRepository.GetByTag("Work");

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains(workContact1, result);
        Assert.Contains(workContact2, result);
        Assert.DoesNotContain(familyContact, result);
    }

    [Fact]
    public void GetByTag_WithLeadingAndTrailingWhitespace_ShouldTrimQuery()
    {
        // Arrange
        var context = new InMemoryDataContext();
        var commandRepository =
            new ContactCommandRepository(context);

        var queryRepository =
            new ContactQueryRepository(context);

        var contact = Contact.Create(
            "Pouya",
            "Mahboubi",
            "09121234567",
            "Work");

        commandRepository.Add(contact);

        // Act
        var result =
            queryRepository.GetByTag("  Work  ");

        // Assert
        var matchedContact = Assert.Single(result);

        Assert.Same(contact, matchedContact);
    }

    [Fact]
    public void GetByTag_ShouldBeCaseSensitive()
    {
        // Arrange
        var context = new InMemoryDataContext();
        var commandRepository =
            new ContactCommandRepository(context);

        var queryRepository =
            new ContactQueryRepository(context);

        var contact = Contact.Create(
            "Pouya",
            "Mahboubi",
            "09121234567",
            "Work");

        commandRepository.Add(contact);

        // Act
        var result =
            queryRepository.GetByTag("work");

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void GetByTag_WhenNoContactMatches_ShouldReturnEmptyCollection()
    {
        // Arrange
        var context = new InMemoryDataContext();
        var commandRepository =
            new ContactCommandRepository(context);

        var queryRepository =
            new ContactQueryRepository(context);

        commandRepository.Add(
            Contact.Create(
                "Pouya",
                "Mahboubi",
                "09121234567",
                "Work"));

        // Act
        var result =
            queryRepository.GetByTag("Family");

        // Assert
        Assert.Empty(result);
    }
    #endregion

    #region State Sharing Between Command and Query Repositories
    [Fact]
    public void CommandAndQueryRepositories_ShouldShareTheSameContextState()
    {
        // Arrange
        var context = new InMemoryDataContext();

        var commandRepository =
            new ContactCommandRepository(context);

        var queryRepository =
            new ContactQueryRepository(context);

        var contact = Contact.Create(
            "Pouya",
            "Mahboubi",
            "09121234567",
            "Work");

        // Act
        commandRepository.Add(contact);

        // Assert
        var result =
            queryRepository.GetById(contact.Id);

        Assert.Same(contact, result);
    }
    #endregion
}