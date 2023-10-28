using RegistrantApplication.Shared.API.View.Base;
using RegistrantApplication.Shared.Database.Orders;

namespace RegistrantApplication.Shared.API.View;

public class ViewOrders : ViewBase
{
    public List<Order> Orders { get; set; } = new List<Order>();
}