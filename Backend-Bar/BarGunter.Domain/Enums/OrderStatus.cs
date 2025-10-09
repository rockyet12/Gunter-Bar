namespace BarGunter.Domain.Enums;

public enum OrderStatus : byte
{
    Pending = 1,
    InPreparation = 2,
    OnTheWay = 3,
    Delivered = 4,
    Cancelled = 5
}
