using PhoneBook.Core.Domain.Contacts;

namespace PhoneBook.Core.Contracts.Contacts.Abstractions.Repositories;

public interface IContactQueryRepository
{
    IReadOnlyList<Contact> GetList();
    Contact? GetById(Guid contactId);
    IReadOnlyList<Contact> GetByTag(string tag);
}