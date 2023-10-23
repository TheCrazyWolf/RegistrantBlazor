
using System.ComponentModel.DataAnnotations;
using RegistrantApplication.Shared.Database.Accounts;

namespace RegistrantApplication.Shared.Database.Drivers
{
    public class Document
    {
        [Key]
        public long IdDocument { get; set; }
        public required string Title { get; set; }
        
        public string? Serial { get; set; } 
        public string? Number { get; set; }
        public DateOnly DateOfIssue { get; set; }
        public string? Authority { get; set; }
        public Account? Account { get; set; }
        
        public FileDocument? FileDocument { get; set; }
        
        public bool IsDeleted { get; set; }
    }

    public enum DocumenType
    {
        [Display(Name = "ПАСПОРТ")]
        Passport,
        [Display(Name = "ВОД. УДОСТ")]
        DriverLicense
    }
}
