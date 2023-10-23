using System.ComponentModel.DataAnnotations;

namespace RegistrantApplication.Shared.Database.Drivers;

public class FileDocument
{
    [Key]
    public long IdFile { get; set; }
    public required string FileName { get; set; }
    public required byte[] DataBytes { get; set; }
}