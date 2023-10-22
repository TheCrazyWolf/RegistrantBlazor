using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistrantApplication.Server.Controllers.Base;
using RegistrantApplication.Server.Database;
using RegistrantApplication.Shared.Accounts;
using RegistrantApplication.Shared.API.Accounts;
using RegistrantApplication.Shared.Drivers;

namespace RegistrantApplication.Server.Controllers;

[ApiController]
[Microsoft.AspNetCore.Mvc.Route("[controller]")]
public class Security : BaseController
{
    public Security(ILogger<BaseController> logger, LiteContext ef) : base(logger, ef)
    {
    }

    [HttpGet("GetAccountDetails")]
    public async Task<IActionResult> GetAccountDetails()
    {
        if (!IsValidateToken().Result)
            return Unauthorized("Требуется авторизация");

        if(_session == null)
            return Unauthorized("Требуется авторизация");
        
        AccountDetails details = new AccountDetails()
        {
            Family = _session.Account.Family.ToUpper(),
            Name = _session.Account.Name.ToUpper(),
            Patronymic = _session.Account.Patronymic?.ToUpper(),
            IsEmployee = _session.Account.IsEmployee
        };

        return Ok(details);
    }

    [HttpGet("CreateTestAccount")]
    public async Task<IActionResult> CreateTestAccount(string family, string name, string phone, string password, bool isEmployee)
    {
        Account account = new Account()
        {
            Family = family,
            Name = name,
            PasswordHash = GetMd5(password).Result,
            PhoneNumber = phone,
            IsEmployee = isEmployee
        };
        await _ef.AddAsync(account);
        await _ef.SaveChangesAsync();
        return Ok();
    }
    
    [HttpGet("GetToken")]
    public async Task<IActionResult> GetToken(string phone, string? password, string? family, bool isEmployee)
    {
        Account? foundAccount = null;
        
        if (isEmployee)
        {
            foundAccount = await _ef.Accounts
                .FirstOrDefaultAsync(x =>
                    x.PhoneNumber.ToUpper() == phone.ToUpper() && GetMd5(password).Result == x.PasswordHash &&
                    x.IsEmployee == true);
        }
        
        if (foundAccount == null)
            return Unauthorized("Пользователь не найден");
        
        var session = new Session()
        {
            Token = GetUnqueString().Result,
            Account = foundAccount,
            DateTimeSessionStarted = DateTime.Now,
            DateTimeSessionExpired = DateTime.Now.AddDays(1)
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

    [HttpPost("ResetToken")]
    public async Task<IActionResult> ResetToken([FromBody] string[] arrayTokens)
    {
        foreach (var token in arrayTokens)
        {
            var foundToken = await _ef.AccountsSessions.FirstOrDefaultAsync(x => x.Token == token);
            
            if(foundToken == null)
                continue;
            
            foundToken.DateTimeSessionExpired = DateTime.Now;;
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