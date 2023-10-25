using RegistrantApplication.Shared.API.View;
using RegistrantApplication.Shared.Database.Accounts;

namespace RegistrantApplication.Shared.API.Accounts.Get;

public class GetViewAccounts : ViewBase
{
    public List<GetAccount> Accounts { get; set; }
}