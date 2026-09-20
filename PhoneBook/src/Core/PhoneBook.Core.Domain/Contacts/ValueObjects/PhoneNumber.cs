using PhoneBook.Core.Domain.Common;

namespace PhoneBook.Core.Domain.Contacts.ValueObjects;

public sealed record PhoneNumber
{
    public string Value { get; }

    private PhoneNumber(string value)
    {
        Value = value;
    }

    public static PhoneNumber Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException("Phone number cannot be empty.");
        }

        return new PhoneNumber(value.Trim());
    }

    public override string ToString() => Value;
}
