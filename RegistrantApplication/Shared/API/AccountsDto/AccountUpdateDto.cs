namespace RegistrantApplication.Shared.API.AccountsDto;

public class AccountUpdateDto : AccountViewDto
{
    public string? PasswordHash { get; set; }
}