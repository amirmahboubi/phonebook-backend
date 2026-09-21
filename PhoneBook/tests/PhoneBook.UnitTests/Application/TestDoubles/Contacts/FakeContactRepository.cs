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
        ArgumentNullException.ThrowIfNull(contact);

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
        ArgumentNullException.ThrowIfNull(contact);

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
        ArgumentNullException.ThrowIfNull(contact);

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
        ArgumentNullException.ThrowIfNull(tag);

        return _contacts.Values
            .Where(contact =>
                contact.Tag.Value == tag.Trim())
            .ToArray();
    }

    public void AddExisting(Contact contact)
    {
        ArgumentNullException.ThrowIfNull(contact);

        _contacts[contact.Id] = contact;
    }

    public void Clear()
    {
        _contacts.Clear();

        AddCallCount = 0;
        UpdateCallCount = 0;
        DeleteCallCount = 0;
    }
}