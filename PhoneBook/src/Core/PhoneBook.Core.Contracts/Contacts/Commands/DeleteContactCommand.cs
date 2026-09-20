namespace PhoneBook.Core.Contracts.Contacts.Commands;

public sealed record DeleteContactCommand(
    Guid ContactId);