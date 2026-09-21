using PhoneBook.Core.Contracts.Contacts.Dtos;
using PhoneBook.Core.Contracts.Contacts.Queries;
using PhoneBook.Core.Contracts.Common.Validation;
using PhoneBook.Core.Contracts.Contacts.Abstractions.Repositories;

namespace PhoneBook.Application.Queries.Contacts.GetById;

public sealed class GetContactByIdQueryHandler
{
    private readonly IContactQueryRepository _repository;
    private readonly GetContactByIdQueryValidator _validator;

    public GetContactByIdQueryHandler(
        IContactQueryRepository repository,
        GetContactByIdQueryValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public ContactDto? Handle(GetContactByIdQuery query)
    {
        var validationResult = _validator.Validate(query);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var contact = _repository.GetById(query.ContactId);

        if (contact is null)
            return null;

        return new ContactDto(
            contact.Id,
            contact.FirstName,
            contact.LastName,
            contact.PhoneNumber.Value,
            contact.Tag.Value);
    }
}
