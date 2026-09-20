namespace PhoneBook.Core.Domain.Common.ValueObjects;

public sealed record Tag
{
    public string Value { get; }

    private Tag(string value)
    {
        Value = value;
    }

    public static Tag Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException("Tag cannot be empty.");
        }

        return new Tag(value.Trim());
    }

    public override string ToString() => Value;
}
