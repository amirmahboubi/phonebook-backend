using PhoneBook.Core.Domain.Contacts;
using PhoneBook.Core.Contracts.Contacts.Dtos;
using PhoneBook.Core.Contracts.Contacts.Commands;
using PhoneBook.Core.Contracts.Contacts.Abstractions.Repositories;

namespace PhoneBook.Application.Commands.Contacts.Create;

public sealed class CreateContactCommandHandler
{
    private readonly IContactCommandRepository _repository;
    private readonly CreateContactCommandValidator _validator;

    public CreateContactCommandHandler(
        IContactCommandRepository repository,
        CreateContactCommandValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public ContactDto Handle(
        CreateContactCommand command)
    {
        var validationResult =
            _validator.Validate(command);

        if (!validationResult.IsValid)
        {
            throw new Common.Exceptions.ValidationException(
                validationResult.Errors);
        }

        var contact = Contact.Create(
            command.FirstName,
            command.LastName,
            command.PhoneNumber,
            command.Tag);

        _repository.Add(contact);

        return new ContactDto(
            contact.Id,
            contact.FirstName,
            contact.LastName,
            contact.PhoneNumber.Value,
            contact.Tag.Value);
    }
}
