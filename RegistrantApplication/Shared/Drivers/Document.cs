
using System.ComponentModel.DataAnnotations;

namespace RegistrantApplication.Shared.Drivers
{
    public class Document
    {
        [Key]
        public long IdDocument { get; set; }
        public string Name { get; set; }
        public byte[] Data { get; set; }
    }
}
