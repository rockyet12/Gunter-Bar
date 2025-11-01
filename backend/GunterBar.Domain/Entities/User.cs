using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GunterBar.Domain.Enums;

namespace GunterBar.Domain.Entities;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? LastName { get; set; }

    [Required, EmailAddress, MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    public UserRole Role { get; set; }

    [MaxLength(20)]
    public string RoleName
    {
        get => Role.ToString();
        set
        {
            if (!string.IsNullOrEmpty(value) && Enum.TryParse<UserRole>(value, out var parsed))
                Role = parsed;
        }
    }


    public string? PhoneNumber { get; set; }

    [MaxLength(250)]
    public string? Address { get; set; }

    [MaxLength(2000)]
    public string? ProfileImageUrl { get; set; }

    [MaxLength(500)]
    public string? DeliveryDescription { get; set; }

    public DateTime? BirthDate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public Bar? Bar { get; set; } // Only for vendors
    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public ICollection<Cart> Carts { get; set; } = new List<Cart>();

    [MaxLength(20)]
    public string? Dni { get; set; }

    public int LoginAttempts { get; set; }
    public DateTime? LastLoginAttempt { get; set; }


    [MaxLength(10)]
    public string? PasswordResetCode { get; set; }

    public DateTime? PasswordResetCodeGeneratedAt { get; set; }

    [MaxLength(6)]
    public string? SmsVerificationCode { get; set; }

    public DateTime? SmsVerificationCodeGeneratedAt { get; set; }

    // Constructor
    public User(string name, string email, string passwordHash)
    {
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        Role = UserRole.Customer;
        LoginAttempts = 0;
    }

    // Constructor con apellido
    public User(string name, string lastName, string email, string passwordHash)
    {
        Name = name;
        LastName = lastName;
        Email = email;
        PasswordHash = passwordHash;
        Role = UserRole.Customer;
        LoginAttempts = 0;
    }

    // Constructor con apellido y rol
    public User(string name, string lastName, string email, string passwordHash, UserRole role)
    {
        Name = name;
        LastName = lastName;
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
        LoginAttempts = 0;
    }

    // Constructor sin parámetros requerido por EF Core
    public User()
    {
        Name = string.Empty;
        Email = string.Empty;
        PasswordHash = string.Empty;
    }
}
