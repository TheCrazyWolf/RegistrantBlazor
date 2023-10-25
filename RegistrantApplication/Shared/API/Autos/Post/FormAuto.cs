namespace RegistrantApplication.Shared.API.Autos.Post;

public class FormAuto
{
    public long IdAuto { get; set; }
    public string Title { get; set; }
    public string AutoNumber { get; set; }
    public long IdAccount { get; set; }
    public bool IsDeleted { get; set; }
}