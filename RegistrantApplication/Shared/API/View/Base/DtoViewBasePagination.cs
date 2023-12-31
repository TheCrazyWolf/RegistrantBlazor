﻿namespace RegistrantApplication.Shared.API.View.Base;

public class DtoViewBasePagination : IViewAPI
{
    public long CurrentPage { get; set; }
    public long TotalPages { get; set; }
    public long TotalRecords { get; set; }
    public long MaxRecordsOnPageConst { get; set; }
}