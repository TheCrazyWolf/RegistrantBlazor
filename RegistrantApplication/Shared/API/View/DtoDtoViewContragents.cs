using RegistrantApplication.Shared.API.ContragentsDto;
using RegistrantApplication.Shared.API.View.Base;

namespace RegistrantApplication.Shared.API.View
{
    public class DtoDtoViewContragents : DtoViewBasePagination
    {
        public List<DtoContragent> Contragents { get; set; } = new List<DtoContragent>();
    }
}
