namespace PhoneBook.Core.Contracts.Contacts.Requests;

public sealed record UpdateContactRequest(
    string? FirstName,
    string? LastName,
    string? PhoneNumber,
    string? Tag);