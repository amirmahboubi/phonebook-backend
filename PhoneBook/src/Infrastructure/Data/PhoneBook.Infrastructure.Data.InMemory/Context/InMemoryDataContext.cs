using System.Collections.Concurrent;
using PhoneBook.Core.Domain.Contacts;
using PhoneBook.Infrastructure.Data.InMemory.Contacts;

namespace PhoneBook.Infrastructure.Data.InMemory.Context;

public sealed class InMemoryDataContext
{
    private readonly ConcurrentDictionary<Guid, Contact> _contacts = new();
    private readonly object _seedLock = new();

    private bool _isSeeded;

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

    public void Seed()
    {
        lock (_seedLock)
        {
            if (_isSeeded)
            {
                return;
            }

            foreach (var contact in ContactsSeedData.Create())
            {
                _contacts.TryAdd(
                    contact.Id,
                    contact);
            }

            _isSeeded = true;
        }
    }
}