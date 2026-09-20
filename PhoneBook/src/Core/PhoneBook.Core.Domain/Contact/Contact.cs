namespace PhoneBook.Core.Domain.Contact;

public sealed class Contact
{
    private Contact(string firstName,
                    string lastName,
                    string phoneNumber,
                    string tag)
    {
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        Tag = tag;
    }

    public Guid Id { get; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string PhoneNumber { get; private set; }
    public string Tag { get; private set; }

    public static Contact Create(string firstName,
                                 string lastName,
                                 string phoneNumber,
                                 string tag)
    {
        throw new NotImplementedException();
    }
}
