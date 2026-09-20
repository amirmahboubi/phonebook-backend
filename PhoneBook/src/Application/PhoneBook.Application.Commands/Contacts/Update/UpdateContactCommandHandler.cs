using PhoneBook.Core.Contracts.Contacts.Commands;
using PhoneBook.Application.Commands.Common.Exceptions;
using PhoneBook.Core.Contracts.Contacts.Abstractions.Repositories;

namespace PhoneBook.Application.Commands.Contacts.Update;

public sealed class UpdateContactCommandHandler
{
    private readonly IContactCommandRepository _commandRepository;
    private readonly IContactQueryRepository _queryRepository;
    private readonly UpdateContactCommandValidator _validator;

    public UpdateContactCommandHandler(
        IContactCommandRepository commandRepository,
        IContactQueryRepository queryRepository,
        UpdateContactCommandValidator validator)
    {
        _commandRepository = commandRepository;
        _queryRepository = queryRepository;
        _validator = validator;
    }

    public void Handle(
        UpdateContactCommand command)
    {
        var validationResult =
            _validator.Validate(command);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(
                validationResult.Errors);
        }

        var contact =
            _queryRepository.GetById(command.ContactId);

        if (contact is null)
        {
            throw new ContactNotFoundException(
                command.ContactId);
        }

        contact.Update(
            command.FirstName,
            command.LastName,
            command.PhoneNumber,
            command.Tag);

        _commandRepository.Update(contact);
    }
}
