using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistrantApplication.Server.Configs;
using RegistrantApplication.Server.Controllers.Base;
using RegistrantApplication.Server.Controllers.BaseAPI;
using RegistrantApplication.Server.Database;
using RegistrantApplication.Shared.Drivers;

namespace RegistrantApplication.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class Autos : BaseApiController
{
    public Autos(ILogger<BaseApiController> logger, LiteContext ef) : base(logger, ef)
    {
    }
    
    /// <summary>
    /// Добавление машин за учетной записью
    /// </summary>
    /// <param name="token">Действующий токен</param>
    /// <param name="idAccount">ID аккаунта</param>
    /// <param name="auto">Модель машины</param>
    /// <returns>200 в случае успешного сохранения</returns>
    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromHeader] string token, [FromHeader] long idAccount, [FromBody] Auto auto)
    {
        if (!IsValidateToken(token).Result)
            return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

        var account = await _ef.Accounts
            .Include(x => x.Autos)
            .FirstOrDefaultAsync(x => x.IdAccount == idAccount);

        if (account?.Autos == null)
            return NotFound(ConfigMsg.ValidationElementNotFound);

        account.Autos.Add(MyValidator.GetModel(auto));
        _ef.Update(account);
        await _ef.SaveChangesAsync();
        return Ok();
    }

    /// <summary>
    /// Получение списка машин закрепленных за учетной записью пользователя
    /// </summary>
    /// <param name="token">Действующий токен</param>
    /// <param name="idAccount">ID аккаунта</param>
    /// <returns>Возращает коллекцию машин</returns>
    [HttpGet("Get")]
    public async Task<IActionResult> Get([FromHeader] string token, long idAccount)
    {
        if (!IsValidateToken(token).Result)
            return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

        var account = await _ef.Accounts
            .Include(x => x.Autos)
            .FirstOrDefaultAsync(x => x.IdAccount == idAccount);

        if (account?.Autos == null)
            return NoContent();

        return Ok(account.Autos.ToList());
    }

    /// <summary>
    /// Обновление данных о машине
    /// </summary>
    /// <param name="token">Действующий токен</param>
    /// <param name="auto">Модель машины с сохранением прошлого ID</param>
    /// <returns>200 в случае успешного сохранения</returns>
    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromHeader] string token, [FromBody] Auto auto)
    {
        if (!IsValidateToken(token).Result)
            return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

        var foundAuto = await _ef.Autos
            .FirstOrDefaultAsync(x => x.IdAuto == auto.IdAuto);

        if (foundAuto == null)
            return NoContent();
        
        _ef.Update(MyValidator.GetModel(foundAuto));
        await _ef.SaveChangesAsync();
        return Ok();
    }
    
    /// <summary>
    /// Удаление/Скрытие машины
    /// </summary>
    /// <param name="token">Действующий токен</param>
    /// <param name="idAutos">ID представленных в массиве</param>
    /// <returns>200 в случае успешного удаления</returns>
    [HttpDelete("Delete")]
    public async Task<IActionResult> Delete([FromHeader] string token, [FromBody]long[] idAutos)
    {
        if (!IsValidateToken(token).Result)
            return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

        foreach (var item in idAutos)
        {
            var foundAuto = await _ef.Autos
                .FirstOrDefaultAsync(x => x.IdAuto == item);
            
            if(foundAuto == null)
                continue;

            foundAuto.IsDeleted = true;
            _ef.Update(foundAuto);
            await _ef.SaveChangesAsync();
        }

        return Ok();
    } 
}