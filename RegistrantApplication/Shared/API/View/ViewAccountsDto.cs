using RegistrantApplication.Shared.API.AccountsDto;
using RegistrantApplication.Shared.API.View.Base;

namespace RegistrantApplication.Shared.API.View;

public class ViewAccountsDto : ViewBase
{
    public List<AccountViewDto> Accounts { get; set; }
}