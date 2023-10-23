using System.ComponentModel.DataAnnotations;
using RegistrantApplication.Shared.Database.Accounts;

namespace RegistrantApplication.Shared.Orders
{
    public class OrderDetails
    {
        [Key]
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
        public Account? StoreKeeper { get; set; }
    }
}
