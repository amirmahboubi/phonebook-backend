using PhoneBook.Core.Domain.Contacts;

namespace PhoneBook.Infrastructure.Data.InMemory.Contacts;

public static class ContactsSeedData
{
    public static IReadOnlyList<Contact> Create()
    {
        return
        [
            Contact.Create(
                "Pouya",
                "Mahboubi",
                "09121234567",
                "Work"),

            Contact.Create(
                "Ali",
                "Ahmadi",
                "09129876543",
                "Family"),

            Contact.Create(
                "Sara",
                "Mohammadi",
                "09351234567",
                "Work")
        ];
    }
}
