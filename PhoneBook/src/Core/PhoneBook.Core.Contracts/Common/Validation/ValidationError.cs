namespace PhoneBook.Core.Contracts.Common.Validation;

public sealed record ValidationError(
    string PropertyName,
    string ErrorMessage);