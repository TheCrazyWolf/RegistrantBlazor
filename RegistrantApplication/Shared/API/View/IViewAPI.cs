namespace RegistrantApplication.Shared.API.View;

public interface IViewAPI
{
    long CurrentPage { get; set; }
    long TotalPages { get; set; }
    long TotalRecords { get; set; }
    long MaxRecordsOnPageConst { get; set; }
}