using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistrantApplication.Server.Configs;
using RegistrantApplication.Server.Controllers.BaseAPI;
using RegistrantApplication.Server.Database;
using RegistrantApplication.Shared.Database.Drivers;

namespace RegistrantApplication.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class Documents : BaseApiController
{
    public Documents(ILogger<BaseApiController> logger, LiteContext ef) : base(logger, ef)
    {
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromHeader] string token, [FromHeader] long idAccount, [FromBody] Document document, [FromForm] IFormFile? file)
    {
        if (!IsValidateToken(token, out var session))
            return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

        if (session != null && !session.Account.AccountRole.CanCreateDocument)
            return StatusCode(403, ConfigMsg.NotAllowed);

        var foundAccount = await Ef.Accounts
            .FirstOrDefaultAsync(x => x.IdAccount == idAccount);

        if (foundAccount == null)
            return NotFound(ConfigMsg.ValidationElementNotFound);

        document = MyValidator.GetModel(document);
        document.Account = foundAccount;
        
        Ef.Add(document);
        Ef.Update(foundAccount);
        await Ef.SaveChangesAsync();
        return Ok();
    }

    
    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromHeader] string token, [FromBody] Document document)
    {
        if (!IsValidateToken(token, out var session))
            return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

        if (session != null && !session.Account.AccountRole.CanEditDocument)
            return StatusCode(403, ConfigMsg.NotAllowed);

        var foundDocument = await Ef.AccountsDocuments
            .Include(x => x.FileDocument)
            .FirstOrDefaultAsync(x => x.IdDocument == document.IdDocument);

        if (foundDocument == null)
            return BadRequest(ConfigMsg.ValidationElementNotFound);

        foundDocument = MyValidator.GetModel(document);

        Ef.Update(foundDocument);
        await Ef.SaveChangesAsync();

        return Ok();

    }

    [HttpDelete("Delete")]
    public async Task<IActionResult> Delete([FromHeader] string token, long[] idsDocuments)
    {
        if (!IsValidateToken(token, out var session))
            return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

        if (session != null && !session.Account.AccountRole.CanDeleteDocument)
            return StatusCode(403, ConfigMsg.NotAllowed);

        foreach (var item in idsDocuments)
        {
            var foundDocument = await Ef.AccountsDocuments
                .FirstOrDefaultAsync(x => x.IdDocument == item);
            
            if(foundDocument ==null)
                continue;

            foundDocument.IsDeleted = true;
            Ef.Update(foundDocument);
            await Ef.SaveChangesAsync();
        }

        return Ok();
    }

    [HttpGet("GetDocuments")]
    public async Task<IActionResult> Get([FromHeader] string token, long idAccount, bool showDeleted)
    {
        if (!IsValidateToken(token, out var session))
            return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

        if (session != null && !session.Account.AccountRole.CanViewDocument)
            return StatusCode(403, ConfigMsg.NotAllowed);

        var documents = Ef.AccountsDocuments
            .Include(x => x.Account)
            .Where(x => x.Account != null && x.Account.IdAccount == idAccount && x.IsDeleted == showDeleted)
            .ToList();

        if (documents == null)
            return NotFound(ConfigMsg.ValidationElementNotFound);

        return Ok(documents);

    }

    [HttpGet("GetFile")]
    public async Task<IActionResult> GetFile([FromHeader] string token, long idDocument)
    {
        if (!IsValidateToken(token, out var session))
            return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

        if (session != null && !session.Account.AccountRole.CanViewDocument)
            return StatusCode(403, ConfigMsg.NotAllowed);

        var document = await Ef.AccountsFileDocuments
            .FirstOrDefaultAsync(x => x.IdFile == idDocument);
        
        if (document == null)
            return NotFound(ConfigMsg.ValidationElementNotFound);

        var file = new FileContentResult(document.DataBytes, "application/octet-stream")
        {
            FileDownloadName = document.FileName
        };

        return file;
    }
}