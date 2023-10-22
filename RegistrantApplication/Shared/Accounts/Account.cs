using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace RegistrantApplication.Shared.Drivers
{
    public class Account
    {
        [Key]
        public long IdAccount { get; set; }
        public required string Family { get; set; }
        public required string Name { get; set; }
        public string? Patronymic { get; set; }
        [JsonIgnore] public string? PasswordHash { get; set; }
        public string? PhoneNumber { get; set; }
        public List<Auto>? Autos { get; set; }
        public List<Document>? Documents { get; set; }
        public bool IsEmployee { get; set; }
        public bool IsDeleted { get; set; }
    }
}
