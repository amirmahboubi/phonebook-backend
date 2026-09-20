using PhoneBook.Core.Domain.Contacts;
using PhoneBook.Core.Contracts.Contacts.Abstractions.Repositories;

namespace PhoneBook.UnitTests.Application.TestDoubles.Contacts;

public sealed class FakeContactRepository
    : IContactCommandRepository,
      IContactQueryRepository
{
    private readonly Dictionary<Guid, Contact> _contacts = [];

    public int AddCallCount { get; private set; }

    public int UpdateCallCount { get; private set; }

    public int DeleteCallCount { get; private set; }

    public void Add(Contact contact)
    {
        AddCallCount++;

        if (!_contacts.TryAdd(
                contact.Id,
                contact))
        {
            throw new InvalidOperationException(
                $"Contact '{contact.Id}' already exists.");
        }
    }

    public void Update(Contact contact)
    {
        UpdateCallCount++;

        if (!_contacts.ContainsKey(contact.Id))
        {
            throw new KeyNotFoundException(
                $"Contact '{contact.Id}' was not found.");
        }

        _contacts[contact.Id] = contact;
    }

    public void Delete(Contact contact)
    {
        DeleteCallCount++;

        if (!_contacts.Remove(contact.Id))
        {
            throw new KeyNotFoundException(
                $"Contact '{contact.Id}' was not found.");
        }
    }

    public IReadOnlyList<Contact> GetAll()
    {
        return _contacts.Values.ToArray();
    }

    public Contact? GetById(Guid contactId)
    {
        return _contacts.GetValueOrDefault(contactId);
    }

    public IReadOnlyList<Contact> GetByTag(string tag)
    {
        return _contacts.Values
            .Where(contact => contact.Tag.Value == tag)
            .ToArray();
    }

    public void AddExisting(Contact contact)
    {
        _contacts[contact.Id] = contact;
    }
}
