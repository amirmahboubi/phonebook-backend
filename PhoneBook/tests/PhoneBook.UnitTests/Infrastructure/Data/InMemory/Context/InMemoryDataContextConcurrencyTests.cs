using PhoneBook.Core.Domain.Contacts;
using PhoneBook.Infrastructure.Data.InMemory.Contacts;
using PhoneBook.Infrastructure.Data.InMemory.Context;
using System.Collections.Concurrent;

namespace PhoneBook.UnitTests.Infrastructure.Data.InMemory.Context;

public sealed class InMemoryDataContextConcurrencyTests
{
    [Fact]
    public void ConcurrentAdds_ShouldStoreAllUniqueContacts()
    {
        // Arrange
        var context = new InMemoryDataContext();

        const int contactCount = 500;

        var contacts = Enumerable
            .Range(1, contactCount)
            .Select(index =>
                Contact.Create(
                    $"First{index}",
                    $"Last{index}",
                    $"0912{index:D7}",
                    "Work"))
            .ToArray();

        // Act
        Parallel.ForEach(
            contacts,
            contact => context.TryAdd(contact));

        // Assert
        var storedContacts = context.Contacts;

        Assert.Equal(
            contactCount,
            storedContacts.Count);

        Assert.Equal(
            contactCount,
            storedContacts
                .Select(contact => contact.Id)
                .Distinct()
                .Count());
    }

    [Fact]
    public void ConcurrentReads_ShouldReturnContactsSafely()
    {
        // Arrange
        var context = new InMemoryDataContext();

        const int contactCount = 200;

        var contacts = Enumerable
            .Range(1, contactCount)
            .Select(index =>
                Contact.Create(
                    $"First{index}",
                    $"Last{index}",
                    $"0912{index:D7}",
                    "Work"))
            .ToArray();

        foreach (var contact in contacts)
        {
            context.TryAdd(contact);
        }

        var foundIds = new ConcurrentBag<Guid>();

        // Act
        Parallel.ForEach(
            contacts,
            contact =>
            {
                if (context.TryGet(
                    contact.Id,
                    out var foundContact))
                {
                    foundIds.Add(
                        foundContact!.Id);
                }
            });

        // Assert
        Assert.Equal(
            contactCount,
            foundIds.Count);

        Assert.Equal(
            contactCount,
            foundIds
                .Distinct()
                .Count());
    }

    [Fact]
    public void ConcurrentRemoves_ShouldRemoveAllContacts()
    {
        // Arrange
        var context = new InMemoryDataContext();

        const int contactCount = 200;

        var contacts = Enumerable
            .Range(1, contactCount)
            .Select(index =>
                Contact.Create(
                    $"First{index}",
                    $"Last{index}",
                    $"0912{index:D7}",
                    "Work"))
            .ToArray();

        foreach (var contact in contacts)
        {
            context.TryAdd(contact);
        }

        // Act
        Parallel.ForEach(
            contacts,
            contact =>
                context.TryRemove(
                    contact.Id,
                    out _));

        // Assert
        Assert.Empty(context.Contacts);
    }

    [Fact]
    public void ConcurrentSeed_ShouldInitializeContactsOnlyOnce()
    {
        // Arrange
        var context = new InMemoryDataContext();

        // Act
        Parallel.For(
            0,
            20,
            _ => context.Seed());

        // Assert
        var seededContacts = ContactsSeedData.Create();

        Assert.Equal(
            seededContacts.Count,
            context.Contacts.Count);
    }
}
