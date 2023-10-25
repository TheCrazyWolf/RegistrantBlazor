namespace RegistrantApplication.Shared.API.Autos.Get;

public class GetAuto
{
    public long IdAuto { get; set; }
    public string Title { get; set; }
    public string AutoNumber { get; set; }
    public long IdAccount { get; set; }
    public bool IsDeleted { get; set; }
}