using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistrantApplication.Server.Configs;
using RegistrantApplication.Server.Controllers.BaseAPI;
using RegistrantApplication.Server.Database;
using RegistrantApplication.Shared.API;
using RegistrantApplication.Shared.API.OrdersDto;
using RegistrantApplication.Shared.Database.Orders;

namespace RegistrantApplication.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class Orders : BaseApiController
{
    
    public Orders(ILogger<BaseApiController> logger, LiteContext ef, IMapper mapper) : base(logger, ef, mapper)
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
    /// <param name="form">Заказ</param>
    /// <returns>Возращает 200 - если все хорощо</returns>
    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromHeader] string token, [FromBody] DtoOrderCreate form)
    {
        if (!IsValidateToken(token, out var session))
            return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

        if (!session!.Account.AccountRole!.CanCreateOrders)
            return StatusCode(403, ConfigMsg.NotAllowed);

        Order order = new Order()
        {
            IsDeleted = false,
            DateTimeCreatedOrder = DateTime.Now,
            Account = await _ef.Accounts.FirstOrDefaultAsync(x => x.IdAccount == form.Account),
            Auto = await _ef.AccountsAutos.FirstOrDefaultAsync(x => x.IdAuto == form.Auto),
            Contragent = await _ef.Contragents.FirstOrDefaultAsync(x=> x.IdContragent == form.Contragent),
            OrderDetail = new OrderDetail()
        };

        form.Adapt(order);
        await _ef.AddAsync(order);
        await _ef.SaveChangesAsync();
        return Ok(order.Adapt<DtoOrderCreate>());
    }

    /// <summary>
    /// Обновить информацию о заказе
    /// </summary>
    /// <param name="token">Валидный токен</param>
    /// <param name="order">Заказ</param>
    /// <returns>Возращает 200 - если все хорощо</returns>
    [HttpPost("Update")]
    public async Task<IActionResult> Update([FromHeader] string token, [FromBody] DtoOrderCreate order)
    {
        if (!IsValidateToken(token, out var session))
            return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

        if (!session!.Account.AccountRole!.CanEditOrders)
            return StatusCode(403, ConfigMsg.NotAllowed);

        var foundOrder = await _ef.Orders
            .FirstOrDefaultAsync(x => x.IdOrder == order.IdOrder);

        if (foundOrder == null)
            return NotFound(ConfigMsg.ValidationElementNotFound);

        
        
        
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

        if (!session!.Account.AccountRole!.CanDeleteOrders)
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
    
}