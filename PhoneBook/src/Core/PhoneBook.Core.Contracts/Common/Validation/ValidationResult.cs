namespace PhoneBook.Core.Contracts.Common.Validation;

public sealed record ValidationResult(
    IReadOnlyList<ValidationError> Errors)
{
    public bool IsValid => Errors.Count == 0;

    public static ValidationResult Success { get; } =
        new([]);
}