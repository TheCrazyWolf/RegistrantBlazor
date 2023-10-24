using RegistrantApplication.Shared.API.View;
using RegistrantApplication.Shared.Database.Admin;

namespace RegistrantApplication.Shared.API;

public class ViewEvents : ViewBase
{
    public List<Event> Events { get; set; }
}