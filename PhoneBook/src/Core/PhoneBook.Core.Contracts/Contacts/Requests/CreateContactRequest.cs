namespace PhoneBook.Core.Contracts.Contacts.Requests;

public sealed record CreateContactRequest(
    string? FirstName,
    string? LastName,
    string? PhoneNumber,
    string? Tag);