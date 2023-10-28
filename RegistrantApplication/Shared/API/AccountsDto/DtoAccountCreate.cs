namespace RegistrantApplication.Shared.API.AccountsDto;

public class DtoAccountCreate : DtoAccountView
{
    public string? PasswordHash { get; set; }
}