using PhoneBook.Core.Contracts.Contacts.Queries;
using PhoneBook.Core.Contracts.Common.Validation;

namespace PhoneBook.Application.Queries.Contacts.GetById;

public sealed class GetContactByIdQueryValidator
{
    public ValidationResult Validate(
        GetContactByIdQuery query)
    {
        ArgumentNullException.ThrowIfNull(query);

        if (query.ContactId == Guid.Empty)
        {
            return new ValidationResult(
            [
                new ValidationError(
                    nameof(query.ContactId),
                    "Contact ID is required.")
            ]);
        }

        return ValidationResult.Success;
    }
}
