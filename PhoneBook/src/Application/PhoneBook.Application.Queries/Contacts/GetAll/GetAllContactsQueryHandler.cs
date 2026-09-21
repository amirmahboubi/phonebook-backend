using PhoneBook.Core.Contracts.Contacts.Dtos;
using PhoneBook.Core.Contracts.Contacts.Queries;
using PhoneBook.Core.Contracts.Contacts.Abstractions.Repositories;

namespace PhoneBook.Application.Queries.Contacts.GetAll;

public sealed class GetAllContactsQueryHandler
{
    private readonly IContactQueryRepository _repository;

    public GetAllContactsQueryHandler(
        IContactQueryRepository repository)
    {
        _repository = repository;
    }

    public IReadOnlyList<ContactDto> Handle(GetAllContactsQuery query)
    {
        ArgumentNullException.ThrowIfNull(query);

        return _repository
            .GetAll()
            .Select(contact =>
                new ContactDto(
                    contact.Id,
                    contact.FirstName,
                    contact.LastName,
                    contact.PhoneNumber.Value,
                    contact.Tag.Value))
            .ToArray();
    }
}
