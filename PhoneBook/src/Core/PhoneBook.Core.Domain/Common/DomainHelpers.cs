namespace PhoneBook.Core.Domain.Common;

internal static class DomainHelpers
{
    internal static string NormalizeRequired(string? value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException($"{fieldName} cannot be empty.");

        return value.Trim();
    }
}
