using RegistrantApplication.Shared.API.View;

namespace RegistrantApplication.Shared.API.AccountsDto;

public class ViewAccountsDto : ViewBase
{
    public List<AccountDto> Accounts { get; set; }
}