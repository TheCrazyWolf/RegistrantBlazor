using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistrantApplication.Server.Configs;
using RegistrantApplication.Server.Controllers.BaseAPI;
using RegistrantApplication.Server.Database;
using RegistrantApplication.Shared.Database.Accounts;

namespace RegistrantApplication.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class Security : BaseApiController
{
    public Security(ILogger<BaseApiController> logger, LiteContext ef, IMapper mapper) : base(logger, ef, mapper)
    {
    }
    
    /// <summary>
    /// Создание сесии и передача токена клиенту
    /// </summary>
    /// <param name="phone">Номер телефона как логин</param>
    /// <param name="password">Пароль</param>
    /// <param name="fingerPrinetBrowser"></param>
    /// <returns>Возращает токен в случае успешной авторизации</returns>
    [HttpGet("GetToken")]
    public async Task<IActionResult> GetToken(string phone, string? password, string? fingerPrintBrowser)
    {
        var foundAccount = await _ef.Accounts
            .Include(x=> x.AccountRole)
            .FirstOrDefaultAsync(x =>
                x.PhoneNumber != null &&
                x.PhoneNumber.ToUpper() == ModelTransfer.ValidationNumber(phone) &&
                ModelTransfer.GetMd5(password).Result == x.PasswordHash &&
                x.IsDeleted == false);
        
        if (foundAccount == null)
            return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);
        
        if(!foundAccount.AccountRole.CanLogin)
            return StatusCode(403, ConfigMsg.NotAllowed);
        
        var session = new AccountSession()
        {
            Token = await ModelTransfer.GetUnqueStringForToken(),
            Account = foundAccount,
            DateTimeSessionStarted = DateTime.Now,
            DateTimeSessionExpired = DateTime.Now.AddHours(ConfigSrv.AuthTokenLifeTimInHour),
            FingerPrintIdentity = fingerPrintBrowser
        };

        await _ef.AddAsync(session);
        await _ef.SaveChangesAsync();
        return Ok(session);
    }

    /// <summary>
    /// Завершение текущей сессии
    /// </summary>
    /// <param name="token">Валидный токен</param>
    /// <param name="forceLogout">Принудительных выход везде</param>
    /// <returns>200 - в случае успешного выхода</returns>
    [HttpGet("Logout")]
    public async Task<IActionResult> Logout([FromHeader] string token, bool forceLogout)
    {
        if (!IsValidateToken(token, out _))
            return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

        var foundToken = await _ef.AccountsSessions
            .FirstOrDefaultAsync(x => x.Token == token);

        if (foundToken == null)
            return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

        foundToken.DateTimeSessionExpired = DateTime.Now;
        _ef.Update(foundToken);
        await _ef.SaveChangesAsync();

        if (!forceLogout)
            return Ok();

        var tokenAcitve = _ef.AccountsSessions
            .Include(x => x.Account)
            .Where(x => x.DateTimeSessionExpired >= DateTime.Now)
            .ToList();

        foreach (var tokenCurrent in tokenAcitve)
        {
            tokenCurrent.DateTimeSessionExpired = DateTime.Now;
            _ef.Update(tokenCurrent);
            await _ef.SaveChangesAsync();
        }

        return Ok();
    }

    /// <summary>
    /// Смена пароля текущего аккаунта
    /// </summary>
    /// <param name="token">Валидный токен</param>
    /// <param name="oldPassword">Нехешированный старый пароль</param>
    /// <param name="newPassword">Нехешированный новый пароль</param>
    /// <param name="newPassword2">Нехешированный новый пароль второй раз</param>
    /// <returns></returns>
    [HttpGet("ChangePassword")]
    public async Task<IActionResult> ChangePassword([FromHeader] string token, string oldPassword, string newPassword,
        string newPassword2)
    {
        if (!IsValidateToken(token, out var session))
            return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

        if (session?.Account.PasswordHash != await ModelTransfer.GetMd5(oldPassword))
            return BadRequest("Старый пароль не совпадает с новым");
        
        if(newPassword != newPassword2)
            return BadRequest("Новый пароль не повторятся правильно");

        session.Account.PasswordHash = await ModelTransfer.GetMd5(newPassword);
        _ef.Update(session.Account);
        await _ef.SaveChangesAsync();

        return Ok();
    }
    
    
    [HttpPost("ResetToken")]
    public async Task<IActionResult> ResetToken([FromBody] string[] arrayTokens)
    {
        foreach (var token in arrayTokens)
        {
            var foundToken = await _ef.AccountsSessions.FirstOrDefaultAsync(x => x.Token == token);

            if (foundToken == null)
                continue;

            foundToken.DateTimeSessionExpired = DateTime.Now;
            _ef.Update(foundToken);
            await _ef.SaveChangesAsync();
        }

        return Ok();
    }
}