using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistrantApplication.Server.Configs;
using RegistrantApplication.Server.Controllers.Base;
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

    [HttpGet("Get")]
    public async Task<IActionResult> Get(long idAccount)
    {
        if (!IsValidateToken().Result)
            return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

        var account = await _ef.Accounts
            .Include(x => x.Autos)
            .FirstOrDefaultAsync(x => x.IdAccount == idAccount);

        if (account?.Autos == null)
            return NoContent();

        return Ok(account.Autos.ToList());
    }

    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromBody] Auto auto)
    {
        if (!IsValidateToken().Result)
            return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

        var foundAuto = await _ef.Autos
            .FirstOrDefaultAsync(x => x.IdAuto == auto.IdAuto);

        if (foundAuto == null)
            return NoContent();

        foundAuto.Title = auto.Title?.ToUpper();
        foundAuto.AutoNumber = auto.AutoNumber.ToUpper();
        foundAuto.IsDeleted = auto.IsDeleted;

        _ef.Update(foundAuto);
        await _ef.SaveChangesAsync();
        return Ok();
    }
    
    [HttpDelete("Delete")]
    public async Task<IActionResult> Delete([FromBody]long[] idAutos)
    {
        if (!IsValidateToken().Result)
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