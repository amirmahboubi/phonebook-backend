namespace PhoneBook.Application.Commands.Common.Exceptions;

public sealed class ContactNotFoundException : Exception
{
    public ContactNotFoundException(Guid contactId)
        : base($"Contact with ID '{contactId}' was not found.")
    {
        ContactId = contactId;
    }

    public Guid ContactId { get; }
}