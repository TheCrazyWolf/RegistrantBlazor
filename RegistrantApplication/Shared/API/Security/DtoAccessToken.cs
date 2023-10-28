namespace RegistrantApplication.Shared.API.Security;

public class DtoAccessToken
{
    public string Token { get; set; }
    public DateTime DateTimeSessionStarted { get; set; }
    public DateTime DateTimeSessionExpired { get; set; }
}