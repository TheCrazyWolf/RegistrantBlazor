using System.ComponentModel.DataAnnotations;
using RegistrantApplication.Shared.Database.Accounts;
using RegistrantApplication.Shared.Database.Contragents;
using RegistrantApplication.Shared.Database.Drivers;

namespace RegistrantApplication.Shared.Database.Orders
{
    public class Order
    {
        [Key]
        public long IdOrder { get; set; }
        public Contragent? Contragent { get; set; }
        public Account? Account { get; set; }
        public Auto? Auto { get; set; }

        public DateTime DateTimeCreatedOrder { get; set; }
        public DateTime DateTimePlannedArrive { get; set; }
        public DateTime? DateTimeRegistration { get; set; }
        public DateTime? DateTimeArrived { get; set; }
        public DateTime? DateTimeStartOrder { get; set; }
        public DateTime? DateTimeEndOrder { get; set; }
        public DateTime? DateTimeLeft { get; set; }
        
        public required OrderDetail OrderDetail { get; set; }
        
        public bool IsDeleted { get; set; }
    }
}
