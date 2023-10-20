using RegistrantApplication.Shared.Drivers;

namespace RegistrantApplication.Shared.API;

public class ViewDrivers
{
    public long CurrentPage { get; set; }
    public long TotalPages { get; set; }
    public long TotalRecords { get; set; }
    public long MaxRecordsOnPageConst { get; set; }
    public List<Account> Accounts { get; set; }
}