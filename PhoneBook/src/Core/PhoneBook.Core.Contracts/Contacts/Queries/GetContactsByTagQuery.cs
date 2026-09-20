namespace PhoneBook.Core.Contracts.Contacts.Queries;

public sealed record GetContactsByTagQuery(
    string? Tag);