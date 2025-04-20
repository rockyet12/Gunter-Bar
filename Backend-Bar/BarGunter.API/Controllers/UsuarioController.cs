using Microsoft.AspNetCore.Mvc;

namespace BarGunter.API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Produces("application/json")]
[Consumes("application/json")]
public class UsuarioController : ControllerBase
{
    [HttpGet]
    public IActionResult GetUsuarios()
    {
        return Ok(new { Message = "cerru" });
    }

    [HttpGet]
    public IActionResult GetUsuarioById(int id)
    {
        return Ok(new { Message = $"Usuario con ID {id}" });
    }

    [HttpPost]
    public IActionResult CreateUsuario([FromBody] object usuario)
    {
        return Created("", new { Message = "Usuario creado", Usuario = usuario });
    }

    [HttpPut]
    public IActionResult UpdateUsuario(int id, [FromBody] object usuario)
    {
        return Ok(new { Message = $"Usuario con ID {id} actualizado", Usuario = usuario });
    }

    [HttpDelete]
    public IActionResult DeleteUsuario(int id)
    {
        return Ok(new { Message = $"Usuario con ID {id} eliminado" });
    }

    [HttpPost]
    public IActionResult Login([FromBody] object loginRequest)
    {
        return Ok(new{message ="Login Exitoso", loginRequest});
    }
}