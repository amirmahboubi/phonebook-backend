namespace PhoneBook.Core.Contracts.Contacts.Commands;

public sealed record UpdateContactCommand(
    Guid ContactId,
    string? FirstName,
    string? LastName,
    string? PhoneNumber,
    string? Tag);