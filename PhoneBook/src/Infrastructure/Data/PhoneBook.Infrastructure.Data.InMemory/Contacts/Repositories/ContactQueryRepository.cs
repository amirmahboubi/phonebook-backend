using PhoneBook.Core.Contracts.Contacts.Abstractions.Repositories;
using PhoneBook.Core.Domain.Contacts;

namespace PhoneBook.Infrastructure.Data.InMemory.Contacts.Repositories;

public sealed class ContactQueryRepository : IContactQueryRepository
{
    private readonly InMemoryDataContext _context;

    public ContactQueryRepository(
        InMemoryDataContext context)
    {
        _context = context;
    }

    public IReadOnlyList<Contact> GetAll()
    {
        return _context
            .Contacts
            .ToList();
    }

    public Contact? GetById(Guid contactId)
    {
        return _context.TryGet(
                contactId,
                out var contact)
            ? contact
            : null;
    }

    public IReadOnlyList<Contact> GetByTag(string tag)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tag);

        var normalizedTag = tag.Trim();

        return _context
            .Contacts
            .Where(contact =>
                contact.Tag.Value == normalizedTag)
            .ToList();
    }
}
