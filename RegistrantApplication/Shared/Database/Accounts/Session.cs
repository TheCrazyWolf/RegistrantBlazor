using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RegistrantApplication.Shared.Database.Accounts;

public class Session
{
    [Key]
    public required string Token { get; set; }
    [JsonIgnore] public Account Account { get; set; }
    public DateTime DateTimeSessionStarted { get; set; }
    public DateTime DateTimeSessionExpired { get; set; }
    
    public string? FingerPrintIdentity { get; set; }
}