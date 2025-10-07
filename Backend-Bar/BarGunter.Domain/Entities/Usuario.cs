using System.ComponentModel.DataAnnotations;
using BarGunter.Domain.Enums;

namespace BarGunter.Domain.Entities;

public class Usuario
{
    [Key]
    public int Id { get; set; }
    public int Dni { get; set; }
    public string NickName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; } // <-- Agrega esta línea para la contraseña
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Direccion { get; set; }
    public DateOnly FDnacimiento { get; set; }
    public Genero Genero { get; set; }
    public Rol Rol { get; set; }
    
    public Usuario()
    {
        Id = 0;
        Dni = 0;
        NickName = string.Empty;
        Email = string.Empty;
        Password = string.Empty;
        Nombre = string.Empty;
        Apellido = string.Empty;
        Direccion = string.Empty;
        FDnacimiento = DateOnly.FromDateTime(DateTime.Now);
        Genero = Genero.Masculino;
        Rol = Rol.Cliente;
    }

    public Usuario(int id, int dni, string nickName, string email, string password, string nombre, string apellido, string direccion, DateOnly fNacimiento, Genero genero, Rol rol)
    {
        Id = id;
        Dni = dni;
        NickName = nickName;
        Email = email;
        Password = password; 
        Nombre = nombre;
        Apellido = apellido;
        Direccion = direccion;
        FDnacimiento = fNacimiento;
        Genero = genero;
        Rol = rol;
    }
}