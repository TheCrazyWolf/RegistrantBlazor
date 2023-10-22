using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistrantApplication.Server.Configs;
using RegistrantApplication.Server.Controllers.Base;
using RegistrantApplication.Server.Controllers.BaseAPI;
using RegistrantApplication.Server.Database;
using RegistrantApplication.Shared.Accounts;
using RegistrantApplication.Shared.API.Accounts;
using RegistrantApplication.Shared.Drivers;

namespace RegistrantApplication.Server.Controllers;

[ApiController]
[Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
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
    /// <param name="isEmployee">Указать вход как сотрудник?</param>
    /// <returns>Возращает токен в случае успешной авторизации</returns>
    [HttpGet("GetToken")]
    public async Task<IActionResult> GetToken(string phone, string? password, bool isEmployee)
    {
        Account? foundAccount = null;

        if (isEmployee)
        {
            foundAccount = await _ef.Accounts
                .FirstOrDefaultAsync(x =>
                    x.PhoneNumber.ToUpper() == MyValidator.ValidationNumber(phone) &&
                    GetMd5(password).Result == x.PasswordHash &&
                    x.IsEmployee == true);
        }

        if (foundAccount == null)
            return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

        var session = new Session()
        {
            Token = GetUnqueString().Result,
            Account = foundAccount,
            DateTimeSessionStarted = DateTime.Now,
            DateTimeSessionExpired = DateTime.Now.AddHours(ConfigServer.AuthTokenLifeTimInHour)
        };

        var sessionResult = new SessionResult()
        {
            Token = session.Token,
            DateTimeSessionStarted = session.DateTimeSessionStarted,
            DateTimeSessionExpired = session.DateTimeSessionExpired,
            IsEmployee = foundAccount.IsEmployee
        };

        await _ef.AddAsync(session);
        await _ef.SaveChangesAsync();
        return Ok(sessionResult);
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
        if (!IsValidateToken(token).Result)
            return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

        var foundToken = await _ef.AccountsSessions.FirstOrDefaultAsync(x => x.Token == token);

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

    [HttpPost("ResetToken")]
    public async Task<IActionResult> ResetToken([FromBody] string[] arrayTokens)
    {
        foreach (var token in arrayTokens)
        {
            var foundToken = await _ef.AccountsSessions.FirstOrDefaultAsync(x => x.Token == token);

            if (foundToken == null)
                continue;

            foundToken.DateTimeSessionExpired = DateTime.Now;
            ;
            _ef.Update(foundToken);
            await _ef.SaveChangesAsync();
        }

        return Ok();
    }


    [NonAction]
    public static async Task<string> GetMd5(string input)
    {
        using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
        {
            await Task.Delay(0);
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            string data = Convert.ToHexString(hashBytes);
            return data; // .NET 5 +
            // Convert the byte array to hexadecimal string prior to .NET 5
            // StringBuilder sb = new System.Text.StringBuilder();
            // for (int i = 0; i < hashBytes.Length; i++)
            // {
            //     sb.Append(hashBytes[i].ToString("X2"));
            // }
            // return sb.ToString();
        }
    }

    [NonAction]
    private static async Task<string> GetUnqueString()
    {
        await Task.Delay(0);
        char[] uniqString = (Guid.NewGuid() + Guid.NewGuid().ToString()).Replace("-", string.Empty).ToArray();

        return new string(uniqString).ToString();
    }
}