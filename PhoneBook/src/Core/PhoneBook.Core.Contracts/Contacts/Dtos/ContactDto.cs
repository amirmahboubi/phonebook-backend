namespace PhoneBook.Core.Contracts.Contacts.Dtos;

public sealed record ContactDto(
    Guid Id,
    string FirstName,
    string LastName,
    string PhoneNumber,
    string Tag);