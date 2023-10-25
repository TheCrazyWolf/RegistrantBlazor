using System.ComponentModel.DataAnnotations;
using RegistrantApplication.Shared.Database.Accounts;

namespace RegistrantApplication.Shared.Database.Drivers
{
    public class Auto
    {
        [Key]
        public long IdAuto { get; set; }
        public string Title { get; set; }
        public  string AutoNumber { get; set; }
        public Account? Account { get; set; }
        public bool IsDeleted { get; set; }
    }
}
