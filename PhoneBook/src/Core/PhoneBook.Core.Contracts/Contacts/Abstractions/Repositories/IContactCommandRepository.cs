using PhoneBook.Core.Domain.Contacts;

namespace PhoneBook.Core.Contracts.Contacts.Abstractions.Repositories;

public interface IContactCommandRepository
{
    void Add(Contact contact);
    void Update(Contact contact);
    void Delete(Contact contact);
}