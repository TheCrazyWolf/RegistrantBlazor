using System.ComponentModel.DataAnnotations;

namespace RegistrantApplication.Shared.Database.Accounts
{
    public class Document
    {
        [Key]
        public long IdDocument { get; set; }
        public string Title { get; set; }
        public string? Serial { get; set; } 
        public string? Number { get; set; }
        public DateOnly DateOfIssue { get; set; }
        public string? Authority { get; set; }
        public Account? Account { get; set; }
        public FileDocument? FileDocument { get; set; }
        public bool IsDeleted { get; set; }
    }
}
