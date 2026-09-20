using PhoneBook.Core.Contracts.Common.Validation;

namespace PhoneBook.Application.Commands.Common.Exceptions;

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