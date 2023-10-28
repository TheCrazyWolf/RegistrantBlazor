using RegistrantApplication.Shared.API.View.Base;
using RegistrantApplication.Shared.Database.Admin;

namespace RegistrantApplication.Shared.API.View;

public class ViewEvents : ViewBase
{
    public List<Event> Events { get; set; } = new List<Event>();
}