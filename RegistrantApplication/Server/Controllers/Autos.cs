using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistrantApplication.Server.Controllers.Base;
using RegistrantApplication.Server.Database;

namespace RegistrantApplication.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class Autos : BaseController
{
  
    public Autos(ILogger<BaseController> logger, LiteContext ef) : base(logger, ef)
    {
    }

    [HttpGet("Get")]
    public async Task<IActionResult> Get(long idAccount)
    {
        if (!IsValidateToken().Result)
            return Unauthorized("Требуется авторизация");

        var account = await _ef.Accounts
            .Include(x => x.Autos)
            .FirstOrDefaultAsync(x => x.IdAccount == idAccount);

        if (account?.Autos == null)
            return NoContent();

        return Ok(account.Autos.ToList());
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(long[] idAutos)
    {
        if (!IsValidateToken().Result)
            return Unauthorized("Требуется авторизация");
        
        
    } 
}