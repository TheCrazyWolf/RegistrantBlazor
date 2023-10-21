namespace RegistrantApplication.Shared.API.Accounts;

public class SessionResult
{
    public string Token { get; set; }
    public DateTime DateTimeSessionStarted { get; set; }
    public DateTime DateTimeSessionExpired { get; set; }
    public bool IsEmployee { get; set; }
}