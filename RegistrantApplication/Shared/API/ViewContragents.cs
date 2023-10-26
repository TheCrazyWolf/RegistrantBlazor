using RegistrantApplication.Shared.API.Contragents;
using RegistrantApplication.Shared.API.View;

namespace RegistrantApplication.Shared.API
{
    public class ViewContragents : ViewBase
    {
        public List<ContragentDto> Contragents { get; set; }
    }
}
