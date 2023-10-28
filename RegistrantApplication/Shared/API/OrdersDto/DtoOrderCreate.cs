namespace RegistrantApplication.Shared.API.OrdersDto;

public class DtoOrderCreate
{
    public long IdOrder { get; set; }
    public long? Contragent { get; set; }
    public long? Account { get; set; }
    public long? Auto { get; set; }

    public DateTime DateTimeCreatedOrder { get; set; }
    public DateTime DateTimePlannedArrive { get; set; }
    public DateTime? DateTimeRegistration { get; set; }
    public DateTime? DateTimeArrived { get; set; }
    public DateTime? DateTimeStartOrder { get; set; }
    public DateTime? DateTimeEndOrder { get; set; }
    public DateTime? DateTimeLeft { get; set; }
}