using System.ComponentModel.DataAnnotations;

namespace RegistrantApplication.Shared.Database.Accounts;

public class FileDocument
{
    [Key]
    public long IdFile { get; set; }
    public string FileName { get; set; }
    public  byte[] DataBytes { get; set; }
}