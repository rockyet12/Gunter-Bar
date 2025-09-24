namespace BarGunter.Application.DTOs;

public class RegisterRequest
{
    public string Nombre { get; set; } // <-- Asegúrate de que esta línea exista
    public string Email { get; set; }
    public string Password { get; set; }
    public int Dni { get; set; }
    // Puedes agregar más propiedades si son necesarias para el registro
}