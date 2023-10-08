using System.ComponentModel.DataAnnotations;

namespace RegistrantApplication.Shared.Drivers
{
    public class Auto
    {
        [Key]
        public long IdAuto { get; set; }
        public string Title { get; set; }
        public string AutoNumber { get; set; }
    }
}
