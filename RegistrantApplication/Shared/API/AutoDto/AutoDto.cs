namespace RegistrantApplication.Shared.API.AutoDto;

public class AutoDto
{
    public long IdAuto { get; set; }
    private string _title;

    public string Title
    {
        get => _title.ToUpper();
        set => _title = value.ToUpper();
    }

    private string _autoNumber;

    public string AutoNumber
    {
        get => _autoNumber.ToUpper();
        set => _autoNumber = value.ToUpper();
    }

    public long IdAccount { get; set; }
}