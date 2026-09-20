using PhoneBook.Core.Domain.Contacts;
using System.Collections.Concurrent;

namespace PhoneBook.Infrastructure.Data.InMemory;

public sealed class InMemoryDataContext
{
    private readonly ConcurrentDictionary<Guid, Contact> _contacts = new();

    public IReadOnlyCollection<Contact> Contacts =>
        _contacts.Values.ToArray();

    public bool TryAdd(Contact contact)
    {
        ArgumentNullException.ThrowIfNull(contact);

        return _contacts.TryAdd(
            contact.Id,
            contact);
    }

    public bool TryGet(
        Guid contactId,
        out Contact? contact)
    {
        return _contacts.TryGetValue(
            contactId,
            out contact);
    }

    public bool TryRemove(
        Guid contactId,
        out Contact? contact)
    {
        return _contacts.TryRemove(
            contactId,
            out contact);
    }
}