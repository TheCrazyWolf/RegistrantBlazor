using System.ComponentModel.DataAnnotations;
using RegistrantApplication.Shared.Database.Accounts;

namespace RegistrantApplication.Shared.Database.Admin;

public class Event
{
    [Key]
    public long IdEvent { get; set; }
    public DateTime DateTimeEvent { get; set; }
    public required Account Account { get; set; }
    public EventType EventType { get; set; }
    public string? ValueObject { get; set; }
    public string? ValueBefore { get; set; }
    public string? ValueAfter { get; set; }
}

public enum EventType
{
    [Display(Name = "Создание")]
    Create,
    [Display(Name = "Обновление")]
    Update,
    [Display(Name = "Удаление")]
    Delete,
}