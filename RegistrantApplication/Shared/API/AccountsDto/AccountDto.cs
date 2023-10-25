namespace RegistrantApplication.Shared.API.AccountsDto;

public class AccountDto
{
    public long? IdAccount { get; set; }
    public string Family { get; set; }
    public string Name { get; set; }
    public string? Patronymic { get; set; }
    public string? PhoneNumber { get; set; }
    public bool IsEmployee { get; set; }
    public bool IsDeleted { get; set; }
}