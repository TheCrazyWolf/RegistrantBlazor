using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistrantApplication.Server.Configs;
using RegistrantApplication.Server.Controllers.BaseAPI;
using RegistrantApplication.Server.Database;
using RegistrantApplication.Shared.API.FilesDto;
using RegistrantApplication.Shared.Database.Accounts;

namespace RegistrantApplication.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class Files : BaseApiController
{
    public Files(ILogger<BaseApiController> logger, LiteContext ef, IMapper mapper) : base(logger, ef, mapper)
    {
    }
    
    /// <summary>
    /// Загрузить файл
    /// </summary>
    /// <param name="token">Валидный токен</param>
    /// <param name="idDocument">ID документа который загружаем</param>
    /// <returns>Готовый документ для скачивания</returns>
    [HttpGet("Get")]
    public async Task<IActionResult> Get([FromHeader] string token, long idFile)
    {
        if (!IsValidateToken(token, out var session))
            return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

        if (!session!.Account.AccountRole!.CanViewDocuments)
            return StatusCode(403, ConfigMsg.NotAllowed);

        var document = await _ef.Files
            .FirstOrDefaultAsync(x => x.IdFile == idFile);
        
        if (document == null)
            return NotFound(ConfigMsg.ValidationElementNotFound);

        var file = new FileContentResult(document.DataBytes, "application/octet-stream")
        {
            FileDownloadName = document.FileName
        };

        return file;
    }


    [HttpPost("Upload")]
    public async Task<IActionResult> Upload([FromHeader] string token, IFormFile? form)
    {
        if (!IsValidateToken(token, out var session))
            return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

        if (!session!.Account.AccountRole!.CanCreateDocuments)
            return StatusCode(403, ConfigMsg.NotAllowed);

        if (form == null || form.Length == 0)
            return BadRequest(ConfigMsg.ValidationTextEmpty);

        if(form.Length > ConfigSrv.MaxFileSize)
            return BadRequest(ConfigMsg.FileOverSize);

        FileDocument newFile;

        await using (var fileStream = form.OpenReadStream())
        {
            newFile = new FileDocument()
            {
                FileName = form.FileName,
                DataBytes = new byte[form.Length]
            };

            await fileStream.ReadAsync(newFile.DataBytes, 0, (int)form.Length);
            
            await _ef.AddAsync(newFile);
            await _ef.SaveChangesAsync();
        }
        
        return Ok(newFile.Adapt<DtoFileInfo>());
    }


    [HttpDelete("Delete")]
    public async Task<IActionResult> Delete([FromHeader] string token, long[] idsFiles)
    {
        if (!IsValidateToken(token, out var session))
            return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

        if (!session!.Account.AccountRole!.CanDeleteDocuments)
            return StatusCode(403, ConfigMsg.NotAllowed);


        foreach (var file in idsFiles)
        {
            var foundFile = await _ef.Files.FirstOrDefaultAsync(x => x.IdFile == file);
            
            if(foundFile == null)
                continue;
            
            _ef.Remove(foundFile);
        }

        await _ef.SaveChangesAsync();
        return Ok();
    }
    
}