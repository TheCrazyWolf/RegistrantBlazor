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
public class Documents : BaseApiController
{
    public Documents(ILogger<BaseApiController> logger, LiteContext ef) : base(logger, ef)
    {
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromHeader] string token, [FromHeader] long idAccount, [FromBody] Document document)
    {
        if (!IsValidateToken(token).Result)
            return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

        var foundAccount = await _ef.Accounts
            .Include(x=> x.Documents)
            .FirstOrDefaultAsync(x => x.IdAccount == idAccount);

        if (foundAccount == null)
            return NotFound(ConfigMsg.ValidationElementNotFound);
        
        foundAccount.Documents?.Add(MyValidator.GetModel(document));
        _ef.Update(foundAccount);
        await _ef.SaveChangesAsync();
        return Ok();
    }
}