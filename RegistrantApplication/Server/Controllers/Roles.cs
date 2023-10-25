using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistrantApplication.Server.Configs;
using RegistrantApplication.Server.Controllers.BaseAPI;
using RegistrantApplication.Server.Database;
using RegistrantApplication.Shared.Database.Accounts;

namespace RegistrantApplication.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class Roles : BaseApiController
{
    public Roles(ILogger<BaseApiController> logger, LiteContext ef) : base(logger, ef)
    {
    }

    /// <summary>
    /// Возращает текущую роль по токену
    /// </summary>
    /// <param name="token">Валидиный токен</param>
    /// <returns>Возращает роль с правами</returns>
    [HttpGet("GetRole")]
    public async Task<IActionResult> GetRole([FromHeader] string token)
    {
        if (!IsValidateToken(token, out var session))
            return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

        var role = await Ef.AccountRoles.FirstOrDefaultAsync(x =>
            session != null && x.IdRole == session.Account.AccountRole.IdRole);

        if (role == null)
            return NotFound(ConfigMsg.ValidationElementNotFound);

        return Ok(role);
    }
    
    /// <summary>
    /// Возращает список ролей доступных в системе
    /// </summary>
    /// <param name="token">Валидиный токен</param>
    /// <returns>Коллекция ролей с правами</returns>
    [HttpGet("GetRoles")]
    public async Task<IActionResult> GetRoles([FromHeader] string token)
    {
        if (!IsValidateToken(token, out var session))
            return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);
        
        if (session != null && !session.Account.AccountRole.CanViewRoles)
            return StatusCode(403, ConfigMsg.NotAllowed);

        var roles = Ef.AccountRoles
            .ToList();
        
        if(roles == null)
            return NotFound(ConfigMsg.ValidationElementNotFound);

        return Ok(roles);
    }


    /// <summary>
    /// Получить роль аккаунта
    /// </summary>
    /// <param name="token">Валидиный токен</param>
    /// <param name="idAccount">ID аккаунта, которого надо получить роль </param>
    /// <returns>Возращает роль с правами пользователя</returns>
    [HttpGet("GetRoleAccount")]
    public async Task<IActionResult> GetRoleAccount([FromHeader] string token, long idAccount)
    {
        if (!IsValidateToken(token, out var session))
            return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);
        
        if (session != null && !session.Account.AccountRole.CanViewRoles)
            return StatusCode(403, ConfigMsg.NotAllowed);

        var role = await Ef.Accounts
            .Include(x => x.AccountRole)
            .FirstOrDefaultAsync(x => x.IdAccount == idAccount);
        
        if(role == null)
            return NotFound(ConfigMsg.ValidationElementNotFound);

        return Ok(role.AccountRole);
    }


    /// <summary>
    /// Создаёт новую роль в системе
    /// </summary>
    /// <param name="token">Валидиный токен</param>
    /// <param name="role">Роль с правами</param>
    /// <returns>Возращает 200 - если все хорошо</returns>
    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromHeader] string token, AccountRole role)
    {
        if (!IsValidateToken(token, out var session))
            return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);
        
        if (session != null && !session.Account.AccountRole.CanCreateRoles)
            return StatusCode(403, ConfigMsg.NotAllowed);

        role = ModelTransfer.GetModel(role);

        Ef.Add(role);
        await Ef.SaveChangesAsync();
        return Ok();
    }


    /// <summary>
    /// Обновление роли в системе
    /// </summary>
    /// <param name="token">Валидиный токен</param>
    /// <param name="role">Роль с правами</param>
    /// <returns>Возращает 200 - если все хорошо</returns>
    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromHeader] string token, AccountRole role)
    {
        if (!IsValidateToken(token, out var session))
            return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);
        
        if (session != null && !session.Account.AccountRole.CanEditRoles)
            return StatusCode(403, ConfigMsg.NotAllowed);
        
        role = ModelTransfer.GetModel(role);
        Ef.Update(role);
        await Ef.SaveChangesAsync();
        return Ok();
    }


    /// <summary>
    /// Удаление ролей в системе
    /// </summary>
    /// <param name="token">Валидиный токен</param>
    /// <param name="idRoles">Массив ролей</param>
    /// <returns>Возращает 200 - если все хорошо</returns>
    [HttpDelete("Delete")]
    public async Task<IActionResult> Delete([FromHeader] string token, long[] idRoles)
    {
        if (!IsValidateToken(token, out var session))
            return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);
        
        if (session != null && !session.Account.AccountRole.CanDeleteRoles)
            return StatusCode(403, ConfigMsg.NotAllowed);

        foreach (var role in idRoles)
        {
            var foundRole = await Ef.AccountRoles.FirstOrDefaultAsync(x => x.IdRole == role);
            if(foundRole == null)
                continue;
            Ef.Remove(foundRole);
        }
        
        await Ef.SaveChangesAsync();
        return Ok();
    }
    
}