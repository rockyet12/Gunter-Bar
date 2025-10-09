namespace BarGunter.Domain.Entities;

using System.ComponentModel.DataAnnotations;

public class DrinkType
{
    [Key]
    public int TypeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public DrinkType() { }

    public DrinkType(int typeId, string name, string description)
    {
        TypeId = typeId;
        Name = name;
        Description = description;
    }
}
