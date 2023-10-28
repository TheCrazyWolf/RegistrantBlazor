namespace RegistrantApplication.Shared.API.OrdersDto;

public class OrderDtoView
{
    public long IdOrder { get; set; }
    public string? ContragentTitle { get; set; }
    public string? AccountTitle { get; set; }
    public string? AutoTitle { get; set; }
    
    public DateTime DateTimeCreatedOrder { get; set; }
    public DateTime DateTimePlannedArrive { get; set; }
    public DateTime? DateTimeRegistration { get; set; }
    public DateTime? DateTimeArrived { get; set; }
    public DateTime? DateTimeStartOrder { get; set; }
    public DateTime? DateTimeEndOrder { get; set; }
    public DateTime? DateTimeLeft { get; set; }
    
}