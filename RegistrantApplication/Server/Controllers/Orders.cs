/*using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistrantApplication.Server.Configs;
using RegistrantApplication.Server.Controllers.BaseAPI;
using RegistrantApplication.Server.Database;
using RegistrantApplication.Shared.Database.Orders;

namespace RegistrantApplication.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class Orders : BaseApiController
{
    public Orders(ILogger<BaseApiController> logger, LiteContext ef) : base(logger, ef)
    {
    }
    
    /// <summary>
    /// Получение списка заказов 
    /// </summary>
    /// <param name="token">Валидный токен</param>
    /// <param name="page">Страница</param>
    /// <param name="showDeleted">Показать удаленные записи</param>
    /// <returns></returns>
    [HttpGet("Get")]
    public async Task<IActionResult> Get([FromHeader] string token, string search, DateOnly startDate, DateTime endDate, 
        long page, bool showDeleted)
    {
        throw new Exception();
        return Ok();
    }
    
    /// <summary>
    /// Создать новый заказ
    /// </summary>
    /// <param name="token">Валидный токен</param>
    /// <param name="order">Заказ</param>
    /// <returns>Возращает 200 - если все хорощо</returns>
    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromHeader] string token, [FromBody] Order order)
    {
        if (!IsValidateToken(token, out var session))
            return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

        if (session != null && !session.Account.AccountRole.CanCreateOrders)
            return StatusCode(403, ConfigMsg.NotAllowed);
        
        order = ModelTransfer.GetModel(order);

        if (order != null && order.OrderDetail == null)
                order.OrderDetail = new OrderDetail();

        _ef.Add(order);
        await _ef.SaveChangesAsync();

        return Ok();
    }

    /// <summary>
    /// Обновить информацию о заказе
    /// </summary>
    /// <param name="token">Валидный токен</param>
    /// <param name="order">Заказ</param>
    /// <returns>Возращает 200 - если все хорощо</returns>
    [HttpPost("Update")]
    public async Task<IActionResult> Update([FromHeader] string token, [FromBody] Order order)
    {
        if (!IsValidateToken(token, out var session))
            return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

        if (session != null && !session.Account.AccountRole.CanEditOrders)
            return StatusCode(403, ConfigMsg.NotAllowed);

        var foundOrder = await _ef.Orders
            .FirstOrDefaultAsync(x => x.IdOrder == order.IdOrder);

        if (foundOrder == null)
            return NotFound(ConfigMsg.ValidationElementNotFound);

        foundOrder = ModelTransfer.GetModel(order);
        
        return Ok();

    }

    /// <summary>
    /// Удаляет заказ
    /// </summary>
    /// <param name="token">Валидный токен</param>
    /// <param name="idsOrder">Массив IDs заказов</param>
    /// <returns>Возращает 200 - если все хорощо</returns>
    [HttpDelete("Delete")]
    public async Task<IActionResult> Delete([FromHeader] string token, [FromBody] long[] idsOrder)
    {
        if (!IsValidateToken(token, out var session))
            return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

        if (session != null && !session.Account.AccountRole.CanDeleteOrders)
            return StatusCode(403, ConfigMsg.NotAllowed);

        foreach (var item in idsOrder)
        {
            var foundOrder = await _ef.Orders.FirstOrDefaultAsync(x => x.IdOrder == item);
            if(foundOrder == null)
                continue;

            foundOrder.IsDeleted = true;
            _ef.Update(foundOrder);
        }

        await _ef.SaveChangesAsync();

        return Ok();
    }
}*/