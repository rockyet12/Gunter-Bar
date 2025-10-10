using System;
using System.Threading.Tasks;
using BarGunter.Application.DTOs;
using BarGunter.Domain.Entities;
using BarGunter.Application.Contracts.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BarGunter.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public UsuarioController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpGet]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Get()
    {
        var usuarios = await _usuarioService.GetAllUsuarios();
        return Ok(usuarios);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Get(int id)
    {
        var usuario = await _usuarioService.GetUsuarioById(id);
        if (usuario == null)
        {
            return NotFound();
        }
        return Ok(usuario);
    }

    [HttpPost]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Post([FromBody] Usuario usuario)
    {
        var id = await _usuarioService.AddUsuario(usuario);
        return CreatedAtAction(nameof(Get), new { id = id }, usuario);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Put(int id, [FromBody] Usuario usuario)
    {
        if (id != usuario.Id)
        {
            return BadRequest();
        }
        var result = await _usuarioService.UpdateUsuario(usuario);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _usuarioService.DeleteUsuario(id);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        var response = await _usuarioService.LoginAsync(loginRequest);
        if (response.Success)
        {
            return Ok(response);
        }
        return Unauthorized(response);
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
    {
        var result = await _usuarioService.RegisterAsync(registerRequest);
        if (result)
        {
            return Ok(new { Message = "Registro exitoso" });
        }
        return BadRequest(new { Message = "El usuario ya existe" });
    }
}