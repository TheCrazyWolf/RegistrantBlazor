using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistrantApplication.Server.Configs;
using RegistrantApplication.Server.Controllers.BaseAPI;
using RegistrantApplication.Server.Database;
using RegistrantApplication.Shared.Database.Orders;

namespace RegistrantApplication.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderDetails : BaseApiController
{
    public OrderDetails(ILogger<BaseApiController> logger, LiteContext ef) : base(logger, ef)
    {
    }
    
    /// <summary>
    /// Создать новую детаель заказа
    /// </summary>
    /// <param name="token">Валидный токен</param>
    /// <param name="idOrder">Номер заказа</param>
    /// <param name="orderDetails"></param>
    /// <returns>Возращает 200 - если все окей</returns>
    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromHeader] string token, [FromHeader] long idOrder, [FromBody] OrderDetail orderDetails)
    {
        if (!IsValidateToken(token, out var session))
            return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);
        
        if (session != null && !session.Account.AccountRole.CanCreateOrderDetails)
            return StatusCode(403, ConfigMsg.NotAllowed);

        var foundOrder = await Ef.Orders
            .Include(order => order.OrderDetail)
            .FirstOrDefaultAsync(x => x.IdOrder == idOrder);

        if (foundOrder == null)
            return NotFound(ConfigMsg.ValidationElementNotFound);

        foundOrder.OrderDetail = ModelTransfer.GetModel(orderDetails);
        Ef.Update(foundOrder);
        await Ef.SaveChangesAsync();
        return Ok();
    }
    
    /// <summary>
    /// Получить Детали заказа
    /// </summary>
    /// <param name="token">Валидный токен</param>
    /// <param name="idOrder">Номер заказа</param>
    /// <returns>Возращает детали заказа</returns>
    [HttpGet("Get")]
    public async Task<IActionResult> Get([FromHeader] string token, long idOrder)
    {
        if (!IsValidateToken(token, out var session))
            return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);
        
        if (session != null && !session.Account.AccountRole.CanViewOrderDetails)
            return StatusCode(403, ConfigMsg.NotAllowed);

        var order = await Ef.Orders
            .Include(x => x.OrderDetail)
            .FirstOrDefaultAsync(x => x.IdOrder == idOrder);

        if (order == null)
            return NotFound(ConfigMsg.ValidationElementNotFound);
        
        return Ok(order.OrderDetail);

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="token"></param>
    /// <param name="orderDetail"></param>
    /// <returns></returns>
    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromHeader] string token, [FromBody] OrderDetail orderDetail)
    {
        if (!IsValidateToken(token, out var session))
            return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);
        
        if (session != null && !session.Account.AccountRole.CanEditOrderDetails)
            return StatusCode(403, ConfigMsg.NotAllowed);

        var orderDetailfFound = await Ef.OrderDetails
            .FirstOrDefaultAsync(x => x.IdOrderDetails == orderDetail.IdOrderDetails);

        orderDetailfFound = ModelTransfer.GetModel(orderDetail);

        Ef.Update(orderDetailfFound);
        await Ef.SaveChangesAsync();

        return Ok();
    }
    
    [HttpDelete("Delete")]
    public async Task<IActionResult> Delete([FromHeader] string token, long[] idsOrderDetails)
    {
        if (!IsValidateToken(token, out var session))
            return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);
        
        if (session != null && !session.Account.AccountRole.CanDeleteOrderDetails)
            return StatusCode(403, ConfigMsg.NotAllowed);

        foreach (var id in idsOrderDetails)
        {
            var foundDetail = await Ef.OrderDetails
                .FirstOrDefaultAsync(x => x.IdOrderDetails == id);
            
            if(foundDetail ==null)
                continue;

            Ef.Remove(foundDetail);

        }

        await Ef.SaveChangesAsync();

        return Ok();

    }
    
}