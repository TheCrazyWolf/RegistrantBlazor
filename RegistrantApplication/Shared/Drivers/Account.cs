using System.ComponentModel.DataAnnotations;

namespace RegistrantApplication.Shared.Drivers
{
    public class Account
    {
        [Key]
        public long IdAccount { get; set; }
        public string Family { get; set; }
        public string Name { get; set; }
        public string? Patronymic { get; set; }
        public string? PasswordHash { get; set; }
        public string? PhoneNumber { get; set; }
        public List<Auto>? Autos { get; set; }
        public List<Document>? Documents { get; set; }
        public bool IsEmployee { get; set; }
        public bool IsDeleted { get; set; }
    }
}
