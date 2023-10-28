namespace RegistrantApplication.Shared.API.OrdersDto;

public class DtoOrderDetails
{
    public long IdOrderDetails { get; set; }
    public string? NumRealese { get; set; }
    public string? CountPodons { get; set; }
    public string? PacketDocuments { get; set; }
    public string? TochkaLoad { get; set; }
    public string? Nomenclature { get; set; }
    public string? Size { get; set; }
    public string? Destination { get; set; }
    public string? TypeLoad { get; set; }
    public string? Description { get; set; }
    public long? IdAccount { get; set; }
}