using PhoneBook.Core.Contracts.Contacts.Abstractions.Repositories;
using PhoneBook.Core.Domain.Contacts;

namespace PhoneBook.Infrastructure.Data.InMemory.Contacts.Repositories;

public sealed class ContactCommandRepository : IContactCommandRepository
{
    private readonly InMemoryDataContext _context;

    public ContactCommandRepository(
        InMemoryDataContext context)
    {
        _context = context;
    }

    public void Add(Contact contact)
    {
        ArgumentNullException.ThrowIfNull(contact);

        if (!_context.TryAdd(contact))
        {
            throw new InvalidOperationException(
                $"A contact with ID '{contact.Id}' already exists.");
        }
    }

    public void Update(Contact contact)
    {
        ArgumentNullException.ThrowIfNull(contact);

        if (!_context.TryGet(
                contact.Id,
                out _))
        {
            throw new KeyNotFoundException(
                $"Contact with ID '{contact.Id}' was not found.");
        }
    }

    public void Delete(Contact contact)
    {
        ArgumentNullException.ThrowIfNull(contact);

        if (!_context.TryRemove(
                contact.Id,
                out _))
        {
            throw new KeyNotFoundException(
                $"Contact with ID '{contact.Id}' was not found.");
        }
    }
}
