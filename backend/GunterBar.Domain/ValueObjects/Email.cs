using System.Text.RegularExpressions;

namespace GunterBar.Domain.ValueObjects;

public class Email
{
    private static readonly Regex EmailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);
    
    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Email Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("El email no puede estar vacío");

        email = email.Trim().ToLower();
        
        if (!EmailRegex.IsMatch(email))
            throw new ArgumentException("El formato del email es inválido");

        return new Email(email);
    }

    public static implicit operator string(Email email) => email.Value;
}
