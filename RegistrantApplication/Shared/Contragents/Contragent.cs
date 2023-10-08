using System.ComponentModel.DataAnnotations;

namespace RegistrantApplication.Shared.Contragents
{
    public class Contragent
    {
        [Key]
        public long IdContragent { get; set; }
        public string Title { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public bool IsDeleted { get; set; }
    }
}
