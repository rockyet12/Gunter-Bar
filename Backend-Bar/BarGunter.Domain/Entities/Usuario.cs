using BarGunter.Domain.Enums;

namespace BarGunter.Domain.Entities;

public class Usuario
{
    public int Id { get; set; }
    public int Dni { get; set; }
    public string NickName { get; set; }
    public string Email { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Direccion { get; set; }
    public DateOnly FDnacimiento { get; set; }
    public Genero Genero { get; set; }
    
    public Usuario()
    {
        Id= 0;
        Dni = 0;
        NickName = string.Empty;
        Email = string.Empty;
        Nombre = string.Empty;
        Apellido = string.Empty;
        Direccion = string.Empty;
        FDnacimiento = DateOnly.FromDateTime(DateTime.Now);
        Genero = Genero.Masculino;
    }

    public Usuario(int id ,int dni, string nickName, string email, string nombre, string apellido, string direccion, DateOnly fNacimiento, Genero genero)
    {
        Id = id;
        Dni = dni;
        NickName = nickName;
        Email = email;
        Nombre = nombre;
        Apellido = apellido;
        Direccion = direccion;
        FDnacimiento = fNacimiento;
        Genero = genero;
    }
}
