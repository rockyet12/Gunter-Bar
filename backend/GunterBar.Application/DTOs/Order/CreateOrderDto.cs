using System.ComponentModel.DataAnnotations;

namespace GunterBar.Application.DTOs.Order;

public class CreateOrderDto
{
    public int UserId { get; set; }

    [StringLength(500, ErrorMessage = "Las notas no pueden tener más de 500 caracteres")]
    public string? Notes { get; set; }
}
