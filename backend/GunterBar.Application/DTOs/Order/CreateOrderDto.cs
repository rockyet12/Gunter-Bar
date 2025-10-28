using System.ComponentModel.DataAnnotations;

namespace GunterBar.Application.DTOs.Order;

public class CreateOrderDto
{
    public int UserId { get; set; }

    [StringLength(500, ErrorMessage = "Las notas no pueden tener más de 500 caracteres")]
    public string? Notes { get; set; }

    [Required]
    public string Direccion { get; set; } = string.Empty;

    [Required]
    public string MetodoPago { get; set; } = string.Empty; // "tarjeta" o "efectivo"

    public string? Tarjeta { get; set; } // número enmascarado o completo

    [Required]
    [StringLength(4, MinimumLength = 4)]
    public string CodigoVerif { get; set; } = string.Empty;
}
