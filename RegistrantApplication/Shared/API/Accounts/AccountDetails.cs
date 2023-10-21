namespace RegistrantApplication.Shared.API.Accounts;

public class AccountDetails
{
    public string Family { get; set; }
    public string Name { get; set; }
    public string? Patronymic { get; set; }
    public bool IsEmployee { get; set; }
}