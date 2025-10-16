using System.ComponentModel;

namespace GunterBar.Domain.Enums;

public enum OrderStatus
{
    [Description("Pending")]
    Pending = 0,

    [Description("In Progress")]
    InProgress = 1,

    [Description("Completed")]
    Completed = 2,

    [Description("Cancelled")]
    Cancelled = 3
}
