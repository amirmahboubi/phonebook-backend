namespace PhoneBook.Core.Contracts.Contacts.Commands;

public sealed record CreateContactCommand(
    string? FirstName,
    string? LastName,
    string? PhoneNumber,
    string? Tag);