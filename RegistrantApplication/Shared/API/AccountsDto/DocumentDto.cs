using RegistrantApplication.Shared.Database.Accounts;

namespace RegistrantApplication.Shared.API.AccountsDto;

public class DocumentDto
{
    public long IdDocument { get; set; }
    private string _title;
    public string Title
    {
        get => _title.ToUpper();
        set => _title = value.ToUpper();
    }
    public DocumentType DocumentType { get; set; }
    public string? Serial { get; set; } 
    public string? Number { get; set; }
    public DateOnly DateOfIssue { get; set; }
    public string? Authority { get; set; }
}