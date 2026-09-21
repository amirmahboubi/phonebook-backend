using PhoneBook.Core.Contracts.Contacts.Queries;
using PhoneBook.Core.Contracts.Common.Validation;

namespace PhoneBook.Application.Queries.Contacts.GetByTag;

public sealed class GetContactsByTagQueryValidator
{
    public ValidationResult Validate(
        GetContactsByTagQuery query)
    {
        ArgumentNullException.ThrowIfNull(query);

        if (string.IsNullOrWhiteSpace(query.Tag))
        {
            return new ValidationResult(
            [
                new ValidationError(
                    nameof(query.Tag),
                    "Tag is required.")
            ]);
        }

        return ValidationResult.Success;
    }
}
