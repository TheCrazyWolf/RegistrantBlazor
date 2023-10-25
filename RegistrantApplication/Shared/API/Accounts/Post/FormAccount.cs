namespace RegistrantApplication.Shared.API.Accounts.Post;

public class FormAccount
{
    public long? IdAccount { get; set; }
    public string Family { get; set; }
    public string Name { get; set; }
    public string? Patronymic { get; set; }
    public string? PasswordHash { get; set; }
    public string? PhoneNumber { get; set; }
    public long IdAccountRole { get; set; }
    public bool IsEmployee { get; set; }
    public bool IsDeleted { get; set; }
}