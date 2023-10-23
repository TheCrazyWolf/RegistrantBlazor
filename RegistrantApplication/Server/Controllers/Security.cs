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
    public Security(ILogger<BaseApiController> logger, LiteContext ef) : base(logger, ef)
    {
    }

    /// <summary>
    /// Создание сесии и передача токена клиенту
    /// </summary>
    /// <param name="phone">Номер телефона как логин</param>
    /// <param name="password">Пароль</param>
    /// <returns>Возращает токен в случае успешной авторизации</returns>
    [HttpGet("GetToken")]
    public async Task<IActionResult> GetToken(string phone, string? password)
    {
        var foundAccount = await Ef.Accounts
            .Include(x=> x.AccountRole)
            .FirstOrDefaultAsync(x =>
                x.PhoneNumber != null &&
                x.PhoneNumber.ToUpper() == MyValidator.ValidationNumber(phone) &&
                MyValidator.GetMd5(password).Result == x.PasswordHash &&
                x.IsDeleted == false);
        
        if (foundAccount == null)
            return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);
        
        if(!foundAccount.AccountRole.CanLogin)
            return StatusCode(403, ConfigMsg.NotAllowed);
        
        var session = new Session()
        {
            Token = await MyValidator.GetUnqueStringForToken(),
            Account = foundAccount,
            DateTimeSessionStarted = DateTime.Now,
            DateTimeSessionExpired = DateTime.Now.AddHours(ConfigServer.AuthTokenLifeTimInHour)
        };

        await Ef.AddAsync(session);
        await Ef.SaveChangesAsync();
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

        var foundToken = await Ef.AccountsSessions.FirstOrDefaultAsync(x => x.Token == token);

        if (foundToken == null)
            return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

        foundToken.DateTimeSessionExpired = DateTime.Now;
        Ef.Update(foundToken);
        await Ef.SaveChangesAsync();

        if (!forceLogout)
            return Ok();

        var tokenAcitve = Ef.AccountsSessions
            .Include(x => x.Account)
            .Where(x => x.DateTimeSessionExpired >= DateTime.Now)
            .ToList();

        foreach (var tokenCurrent in tokenAcitve)
        {
            tokenCurrent.DateTimeSessionExpired = DateTime.Now;
            Ef.Update(tokenCurrent);
            await Ef.SaveChangesAsync();
        }

        return Ok();
    }

    [HttpPost("ResetToken")]
    public async Task<IActionResult> ResetToken([FromBody] string[] arrayTokens)
    {
        foreach (var token in arrayTokens)
        {
            var foundToken = await Ef.AccountsSessions.FirstOrDefaultAsync(x => x.Token == token);

            if (foundToken == null)
                continue;

            foundToken.DateTimeSessionExpired = DateTime.Now;
            Ef.Update(foundToken);
            await Ef.SaveChangesAsync();
        }

        return Ok();
    }

    
}