using RegistrantApplication.Shared.Database.Accounts;

namespace RegistrantApplication.Shared.API.AccountsDto;

public class DtoDocumentAccount
{
    public long IdDocument { get; set; }
    private string _title;
    public string Title
    {
        get => _title.ToUpper();
        set => _title = value.ToUpper();
    }
    private string? _serial;

    public string? Serial
    {
        get => _serial!.ToUpper();
        set => _serial = value!.ToUpper();
    }

    private string? _number;
    public string? Number
    {
        get => _number!.ToUpper();
        set => _number = value!.ToUpper();
    }

    public DateOnly DateOfIssue { get; set; }
    private string? _authority;

    public string? Authority
    {
        get => _authority;
        set => _authority = value!.ToUpper();
    }
    public long? IdAccount { get; set; }
    public long? IdFile { get; set; }
}