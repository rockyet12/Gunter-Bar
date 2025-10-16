using System.ComponentModel.DataAnnotations;
using GunterBar.Domain.Enums;

namespace GunterBar.Application.DTOs.Order;

public class UpdateOrderStatusDto
{
    [Required(ErrorMessage = "El ID de la orden es requerido")]
    public int OrderId { get; set; }

    [Required(ErrorMessage = "El nuevo estado es requerido")]
    public OrderStatus NewStatus { get; set; }

    [StringLength(500, ErrorMessage = "Las notas no pueden tener más de 500 caracteres")]
    public string? Notes { get; set; }
}
