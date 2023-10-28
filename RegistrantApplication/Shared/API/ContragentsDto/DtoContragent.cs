namespace RegistrantApplication.Shared.API.ContragentsDto;

public class DtoContragent
{
    public long IdContragent { get; set; }
    private string _title;

    public string Title
    {
        get => _title.ToUpper();
        set => _title = value.ToUpper();
    }
    
    public DateTime DateTimeCreated { get; set; }
}