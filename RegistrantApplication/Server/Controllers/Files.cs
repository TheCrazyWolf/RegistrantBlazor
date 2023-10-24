using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistrantApplication.Server.Configs;
using RegistrantApplication.Server.Controllers.BaseAPI;
using RegistrantApplication.Server.Database;

namespace RegistrantApplication.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class Files : BaseApiController
{
    public Files(ILogger<BaseApiController> logger, LiteContext ef) : base(logger, ef)
    {
    }
    
    /// <summary>
    /// Загрузить файл
    /// </summary>
    /// <param name="token">Валидный токен</param>
    /// <param name="idDocument">ID документа который загружаем</param>
    /// <returns>Готовый документ для скачивания</returns>
    [HttpGet("GetFile")]
    public async Task<IActionResult> GetFile([FromHeader] string token, long idDocument)
    {
        if (!IsValidateToken(token, out var session))
            return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

        if (session != null && !session.Account.AccountRole.CanViewDocuments)
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