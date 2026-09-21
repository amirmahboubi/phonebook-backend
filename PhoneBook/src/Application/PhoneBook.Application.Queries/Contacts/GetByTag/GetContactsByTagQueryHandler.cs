using PhoneBook.Core.Contracts.Contacts.Dtos;
using PhoneBook.Core.Contracts.Contacts.Queries;
using PhoneBook.Core.Contracts.Common.Validation;
using PhoneBook.Core.Contracts.Contacts.Abstractions.Repositories;

namespace PhoneBook.Application.Queries.Contacts.GetByTag;

public sealed class GetContactsByTagQueryHandler
{
    private readonly IContactQueryRepository _repository;
    private readonly GetContactsByTagQueryValidator _validator;

    public GetContactsByTagQueryHandler(
        IContactQueryRepository repository,
        GetContactsByTagQueryValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public IReadOnlyList<ContactDto> Handle(
        GetContactsByTagQuery query)
    {
        var validationResult =
            _validator.Validate(query);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(
                validationResult.Errors);
        }

        var contacts =
            _repository.GetByTag(query.Tag!);

        return contacts
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
