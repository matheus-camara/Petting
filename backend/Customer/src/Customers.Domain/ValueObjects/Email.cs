using Customers.Domain.Validators;
using FluentValidation;

namespace Customers.Domain.ValueObjects;

public record struct Email
{
    public Email(string value)
    {
        Value = value;
        new EmailValidator().ValidateAndThrow(this);
    }

    public string Value { get; init; }

    public static implicit operator string(Email email)
    {
        return email.Value;
    }

    public static implicit operator Email(string email)
    {
        return new(email);
    }
}