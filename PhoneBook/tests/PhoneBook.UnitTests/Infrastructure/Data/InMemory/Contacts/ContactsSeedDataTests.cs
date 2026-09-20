using PhoneBook.Infrastructure.Data.InMemory.Contacts;

namespace PhoneBook.UnitTests.Infrastructure.Data.InMemory.Contacts;

public sealed class ContactsSeedDataTests
{
    [Fact]
    public void Create_ShouldReturnSeedContacts()
    {
        // Act
        var contacts = ContactsSeedData.Create();

        // Assert
        Assert.NotEmpty(contacts);
    }

    [Fact]
    public void Create_ShouldReturnContactsWithUniqueIds()
    {
        // Act
        var contacts = ContactsSeedData.Create();

        // Assert
        var distinctIds = contacts
            .Select(contact => contact.Id)
            .Distinct()
            .Count();

        Assert.Equal(
            contacts.Count,
            distinctIds);
    }

    [Fact]
    public void Create_ShouldReturnContactsWithValidRequiredValues()
    {
        // Act
        var contacts = ContactsSeedData.Create();

        // Assert
        Assert.All(
            contacts,
            contact =>
            {
                Assert.NotEqual(
                    Guid.Empty,
                    contact.Id);

                Assert.False(
                    string.IsNullOrWhiteSpace(
                        contact.FirstName));

                Assert.False(
                    string.IsNullOrWhiteSpace(
                        contact.LastName));

                Assert.False(
                    string.IsNullOrWhiteSpace(
                        contact.PhoneNumber.Value));

                Assert.False(
                    string.IsNullOrWhiteSpace(
                        contact.Tag.Value));
            });
    }

    [Fact]
    public void Create_ShouldContainExpectedTags()
    {
        // Act
        var contacts = ContactsSeedData.Create();

        // Assert
        Assert.Contains(
            contacts,
            contact => contact.Tag.Value == "Work");

        Assert.Contains(
            contacts,
            contact => contact.Tag.Value == "Family");
    }
}
