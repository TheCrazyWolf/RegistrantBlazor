using System.ComponentModel.DataAnnotations;

namespace RegistrantApplication.Shared.Database.Accounts
{
    public class Auto
    {
        [Key]
        public long IdAuto { get; set; }
        public string Title { get; set; }
        public string AutoNumber { get; set; }
        public Account? Account { get; set; }
        public bool IsDeleted { get; set; }
    }
}
