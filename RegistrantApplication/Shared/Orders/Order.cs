using RegistrantApplication.Shared.Contragents;
using RegistrantApplication.Shared.Drivers;
using System.ComponentModel.DataAnnotations;

namespace RegistrantApplication.Shared.Orders
{
    public class Order
    {
        [Key]
        public long IdOrder { get; set; }
        public Contragent? Contragent { get; set; }
        public Driver? Driver { get; set; }
        public Auto? Auto { get; set; }

        public DateTime DateTimeCreatedOrder { get; set; }
        public DateTime DateTimePlannedArrive { get; set; }
        public DateTime? DateTimeRegistration { get; set; }
        public DateTime? DateTimeArrived { get; set; }
        public DateTime? DateTimeStartOrder { get; set; }
        public DateTime? DateTimeEndOrder { get; set; }
        public DateTime? DateTimeLeft { get; set; }
    }
}
