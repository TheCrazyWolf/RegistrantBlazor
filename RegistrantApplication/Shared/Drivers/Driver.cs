using System.ComponentModel.DataAnnotations;

namespace RegistrantApplication.Shared.Drivers
{
    public class Driver
    {
        [Key]
        public long IdDriver { get; set; }
        public string Family { get; set; }
        public string Name { get; set; }
        public string? Patronymic { get; set; }
        public string? PhoneNumber { get; set; }
        public List<Auto>? Autos { get; set; }
        public List<Document>? Documents { get; set; }
        public bool IsDeleted { get; set; }
    }
}
