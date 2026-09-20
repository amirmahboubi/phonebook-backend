using PhoneBook.Core.Domain.Common;
using PhoneBook.Core.Domain.Common.ValueObjects;

namespace PhoneBook.Core.Domain.Contact;

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
        string normalizedFirstName = DomainHelpers.NormalizeRequired(firstName, nameof(firstName));
        string normalizedLastName = DomainHelpers.NormalizeRequired(lastName, nameof(lastName));
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
        FirstName = DomainHelpers.NormalizeRequired(firstName, nameof(firstName));
        LastName = DomainHelpers.NormalizeRequired(lastName, nameof(lastName));
        PhoneNumber = PhoneNumber.Create(phoneNumber);
        Tag = Tag.Create(tag);
    }
}
