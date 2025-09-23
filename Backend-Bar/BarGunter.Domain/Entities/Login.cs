namespace BarGunter.Domain.Entities;

public class Login
{
    public int IdLogin { get; set; }
    public Usuario idUsuario { get; set; }
    public string Password { get; set; }
    public int Dni { get; set; }

    public Login(int idLogin, Usuario idUsuario, string password, int dni)
    {
        IdLogin = idLogin;
        this.idUsuario = idUsuario;
        Password = password;
        Dni = dni;
    }
}
