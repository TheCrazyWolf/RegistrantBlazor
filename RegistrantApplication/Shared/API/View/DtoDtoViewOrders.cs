using RegistrantApplication.Shared.API.View.Base;
using RegistrantApplication.Shared.Database.Orders;

namespace RegistrantApplication.Shared.API.View;

public class DtoDtoViewOrders : DtoViewBasePagination
{
    public List<Order> Orders { get; set; } = new List<Order>();
}