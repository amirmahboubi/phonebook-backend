using PhoneBook.Core.Contracts.Common.Validation;
using PhoneBook.Core.Contracts.Contacts.Commands;

namespace PhoneBook.Application.Commands.Contacts.Create;

public sealed class CreateContactCommandValidator
{
    public ValidationResult Validate(CreateContactCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);

        var errors = new List<ValidationError>();

        if (string.IsNullOrWhiteSpace(command.FirstName))
        {
            errors.Add(
                new ValidationError(
                    nameof(command.FirstName),
                    "First name is required."));
        }

        if (string.IsNullOrWhiteSpace(command.LastName))
        {
            errors.Add(
                new ValidationError(
                    nameof(command.LastName),
                    "Last name is required."));
        }

        if (string.IsNullOrWhiteSpace(command.PhoneNumber))
        {
            errors.Add(
                new ValidationError(
                    nameof(command.PhoneNumber),
                    "Phone number is required."));
        }

        if (string.IsNullOrWhiteSpace(command.Tag))
        {
            errors.Add(
                new ValidationError(
                    nameof(command.Tag),
                    "Tag is required."));
        }

        return errors.Count == 0
            ? ValidationResult.Success
            : new ValidationResult(errors);
    }
}