using RegistrantApplication.Shared.API.AccountsDto;
using RegistrantApplication.Shared.API.View.Base;

namespace RegistrantApplication.Shared.API.View;

public class DtoDtoViewAccounts : DtoViewBasePagination
{
    public List<DtoAccountView> Accounts { get; set; }
}