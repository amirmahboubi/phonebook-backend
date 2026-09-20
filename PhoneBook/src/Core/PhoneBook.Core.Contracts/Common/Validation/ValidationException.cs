namespace PhoneBook.Core.Contracts.Common.Validation;

public sealed class ValidationException : Exception
{
    public ValidationException(
        IReadOnlyList<ValidationError> errors)
        : base("One or more validation errors occurred.")
    {
        Errors = errors;
    }

    public IReadOnlyList<ValidationError> Errors { get; }
}