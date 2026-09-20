using PhoneBook.Core.Contracts.Common.Validation;
using PhoneBook.Core.Contracts.Contacts.Commands;
using PhoneBook.Application.Commands.Common.Exceptions;
using PhoneBook.Core.Contracts.Contacts.Abstractions.Repositories;

namespace PhoneBook.Application.Commands.Contacts.Delete;

public sealed class DeleteContactCommandHandler
{
    private readonly IContactCommandRepository _commandRepository;
    private readonly IContactQueryRepository _queryRepository;
    private readonly DeleteContactCommandValidator _validator;

    public DeleteContactCommandHandler(
        IContactCommandRepository commandRepository,
        IContactQueryRepository queryRepository,
        DeleteContactCommandValidator validator)
    {
        _commandRepository = commandRepository;
        _queryRepository = queryRepository;
        _validator = validator;
    }

    public void Handle(
        DeleteContactCommand command)
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

        _commandRepository.Delete(contact);
    }
}
