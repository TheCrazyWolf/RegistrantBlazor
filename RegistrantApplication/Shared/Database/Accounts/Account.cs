using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RegistrantApplication.Shared.Database.Accounts
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
        [JsonIgnore] public AccountRole? AccountRole { get; set; }
        public bool IsEmployee { get; set; }
        public bool IsDeleted { get; set; }
    }
}
