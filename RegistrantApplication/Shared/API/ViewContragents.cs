using RegistrantApplication.Shared.Contragents;

namespace RegistrantApplication.Shared.API
{
    public class ViewContragents
    {
        public long CurrentPage { get; set; }
        public long TotalPages { get; set; }
        public long TotalRecords { get; set; }
        public long MaxRecordsOnPageConst { get; set; }
        public List<Contragent> Contragents { get; set; }
    }
}
