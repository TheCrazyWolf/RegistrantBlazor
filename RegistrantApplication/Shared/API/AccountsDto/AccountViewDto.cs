namespace RegistrantApplication.Shared.API.AccountsDto;

public class AccountViewDto
{
    public long? IdAccount { get; set; }
    private string _family { get; set; }

    public string Family
    {
        get => _family.ToUpper();
        set => _family = value.ToUpper();
    }

    private string _name;

    public string Name
    {
        get => _name.ToUpper();
        set => _name = value.ToUpper();
    }

    private string? _patronomic;

    public string? Patronymic
    {
        get => _patronomic?.ToUpper();
        set => _patronomic = value?.ToUpper();
    }

    private string? _phoneNumber;
    public string? PhoneNumber 
    {
        get => _phoneNumber?.ToUpper();
        set => _phoneNumber = value?.ToUpper();
    }

    public long? IdAccountRole { get; set; }
    
    public bool IsEmployee { get; set; }
}