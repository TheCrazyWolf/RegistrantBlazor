using RegistrantApplication.Shared.API.ContragentsDto;
using RegistrantApplication.Shared.API.View.Base;

namespace RegistrantApplication.Shared.API.View
{
    public class ViewContragents : ViewBase
    {
        public List<ContragentDto> Contragents { get; set; } = new List<ContragentDto>();
    }
}
