using PhoneBook.Core.Domain.Common;
using PhoneBook.Core.Domain.Contacts.ValueObjects;

namespace PhoneBook.Core.Domain.Contacts;

public sealed class Contact
{
    private Contact(string firstName,
                    string lastName,
                    PhoneNumber phoneNumber,
                    Tag tag)
    {
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        Tag = tag;
    }

    public Guid Id { get; } = Guid.NewGuid();
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    public Tag Tag { get; private set; }

    public static Contact Create(string firstName,
                                 string lastName,
                                 string phoneNumberValue,
                                 string tagValue)
    {
        string normalizedFirstName = NormalizeRequired(firstName, nameof(firstName));
        string normalizedLastName = NormalizeRequired(lastName, nameof(lastName));
        var phoneNumber = PhoneNumber.Create(phoneNumberValue);
        var tag = Tag.Create(tagValue);

        Contact contact = new(normalizedFirstName, normalizedLastName, phoneNumber, tag);
        return contact;
    }

    public void Update(string? firstName,
                       string? lastName,
                       string? phoneNumber,
                       string? tag)
    {
        var normalizedFirstName = NormalizeRequired(firstName, nameof(firstName));
        var normalizedLastName = NormalizeRequired(lastName, nameof(lastName));
        var updatedPhoneNumber = PhoneNumber.Create(phoneNumber);
        var updatedTag = Tag.Create(tag);

        FirstName = normalizedFirstName;
        LastName = normalizedLastName;
        PhoneNumber = updatedPhoneNumber;
        Tag = updatedTag;
    }

    private static string NormalizeRequired(string? value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException($"{fieldName} cannot be empty.");

        return value.Trim();
    }
}
