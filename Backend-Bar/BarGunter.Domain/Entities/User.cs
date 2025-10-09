using BarGunter.Domain.Enums;

namespace BarGunter.Domain.Entities;

using System.ComponentModel.DataAnnotations;

public class User
{
    [Key]
    public int UserId { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100)]
    public string Password { get; set; } = string.Empty;
    
    public Gender Gender { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Role { get; set; } = "Customer"; // Customer, Admin, Employee
    
    public DateTime CreatedDate { get; set; }
    
    public bool IsActive { get; set; }

    public User()
    {
        CreatedDate = DateTime.Now;
        IsActive = true;
    }

    public User(string name, string email, string password, Gender gender, string role = "Customer")
    {
        Name = name;
        Email = email;
        Password = password;
        Gender = gender;
        Role = role;
        CreatedDate = DateTime.Now;
        IsActive = true;
    }
}
