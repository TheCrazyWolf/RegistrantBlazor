using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistrantApplication.Server.Configs;
using RegistrantApplication.Server.Controllers.BaseAPI;
using RegistrantApplication.Server.Database;
using RegistrantApplication.Shared.Database.Drivers;

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
        if (!IsValidateToken(token, out var session))
            return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

        if (session != null && !session.Account.AccountRole.CanCreateAutos)
            return StatusCode(403, ConfigMsg.NotAllowed);
        
        var account = await Ef.Accounts
            .FirstOrDefaultAsync(x => x.IdAccount == idAccount);

        if (account == null)
            return NotFound(ConfigMsg.ValidationElementNotFound);

        auto = MyValidator.GetModel(auto);
        auto.Account = account;

        Ef.Add(auto);
        await Ef.SaveChangesAsync();
        return Ok();
    }

    /// <summary>
    /// Получение списка машин закрепленных за учетной записью пользователя
    /// </summary>
    /// <param name="token">Действующий токен</param>
    /// <param name="idAccount">ID аккаунта</param>
    /// <param name="showDeleted">Показывать удаленных</param>
    /// <returns>Возращает коллекцию машин</returns>
    [HttpGet("Get")]
    public async Task<IActionResult> Get([FromHeader] string token, long idAccount, bool showDeleted)
    {
        if (!IsValidateToken(token, out var session))
            return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

        if (session != null && !session.Account.AccountRole.CanViewAutos)
            return StatusCode(403, ConfigMsg.NotAllowed);

        var autoList =  Ef.AccountsAutos
            .Include(x => x.Account)
            .Where(x => x.Account != null && x.Account.IdAccount == idAccount && x.IsDeleted == showDeleted)
            .ToList();

        if (autoList == null)
            return NoContent();

        return Ok(autoList);
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
        if (!IsValidateToken(token, out var session))
            return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

        if (session != null && !session.Account.AccountRole.CanEditAutos)
            return StatusCode(403, ConfigMsg.NotAllowed);

        var foundAuto = await Ef.AccountsAutos
            .FirstOrDefaultAsync(x => x.IdAuto == auto.IdAuto);

        if (foundAuto == null)
            return NoContent();
        
        Ef.Update(MyValidator.GetModel(foundAuto));
        await Ef.SaveChangesAsync();
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
        if (!IsValidateToken(token, out var session))
            return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

        if (session != null && !session.Account.AccountRole.CanDeleteAutos)
            return StatusCode(403, ConfigMsg.NotAllowed);

        foreach (var item in idAutos)
        {
            var foundAuto = await Ef.AccountsAutos
                .FirstOrDefaultAsync(x => x.IdAuto == item);
            
            if(foundAuto == null)
                continue;

            foundAuto.IsDeleted = true;
            Ef.Update(foundAuto);
            await Ef.SaveChangesAsync();
        }

        return Ok();
    } 
}