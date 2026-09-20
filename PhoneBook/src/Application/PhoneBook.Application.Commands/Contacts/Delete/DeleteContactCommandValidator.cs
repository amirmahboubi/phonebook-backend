using PhoneBook.Core.Contracts.Common.Validation;
using PhoneBook.Core.Contracts.Contacts.Commands;

namespace PhoneBook.Application.Commands.Contacts.Delete;

public sealed class DeleteContactCommandValidator
{
    public ValidationResult Validate(
        DeleteContactCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        if (command.ContactId == Guid.Empty)
        {
            return new ValidationResult(
            [
                new ValidationError(
                    nameof(command.ContactId),
                    "Contact ID is required.")
            ]);
        }

        return ValidationResult.Success;
    }
}
