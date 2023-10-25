/*using Microsoft.AspNetCore.Mvc;
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

    /// <summary>
    /// Создать документ
    /// </summary>
    /// <param name="token">Валидный токен</param>
    /// <param name="idAccount">ID аккаунта для которого создается документ</param>
    /// <param name="document">Содержимое документа</param>
    /// <param name="file">Загрузка файла</param>
    /// <returns>Возращает 200 - если документ успешно сохранился</returns>
    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromHeader] string token, [FromHeader] long idAccount, [FromBody] Document document, [FromForm] IFormFile? file)
    {
        if (!IsValidateToken(token, out var session))
            return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

        if (session != null && !session.Account.AccountRole.CanCreateDocuments)
            return StatusCode(403, ConfigMsg.NotAllowed);

        var foundAccount = await _ef.Accounts
            .FirstOrDefaultAsync(x => x.IdAccount == idAccount);

        if (foundAccount == null)
            return NotFound(ConfigMsg.ValidationElementNotFound);

        document = ModelTransfer.GetModel(document);
        document.Account = foundAccount;
        
        _ef.Add(document);
        _ef.Update(foundAccount);
        await _ef.SaveChangesAsync();
        return Ok();
    }

    
    /// <summary>
    /// Обновление документа
    /// </summary>
    /// <param name="token">Валидный токен</param>
    /// <param name="document">Содержимое документа</param>
    /// <returns>Возращает 200 - если документ успешно сохранился</returns>
    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromHeader] string token, [FromBody] Document document)
    {
        if (!IsValidateToken(token, out var session))
            return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

        if (session != null && !session.Account.AccountRole.CanEditDocuments)
            return StatusCode(403, ConfigMsg.NotAllowed);

        var foundDocument = await _ef.AccountsDocuments
            .Include(x => x.FileDocument)
            .FirstOrDefaultAsync(x => x.IdDocument == document.IdDocument);

        if (foundDocument == null)
            return BadRequest(ConfigMsg.ValidationElementNotFound);

        foundDocument = ModelTransfer.GetModel(document);

        _ef.Update(foundDocument);
        await _ef.SaveChangesAsync();

        return Ok();

    }

    /// <summary>
    /// Удаляет документ из системы
    /// </summary>
    /// <param name="token">Валидный токен</param>
    /// <param name="idsDocuments">Массив ID документов для удаления</param>
    /// <returns>Возращает 200 - если документ успешно удален</returns>
    [HttpDelete("Delete")]
    public async Task<IActionResult> Delete([FromHeader] string token, long[] idsDocuments)
    {
        if (!IsValidateToken(token, out var session))
            return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

        if (session != null && !session.Account.AccountRole.CanDeleteDocuments)
            return StatusCode(403, ConfigMsg.NotAllowed);

        foreach (var item in idsDocuments)
        {
            var foundDocument = await _ef.AccountsDocuments
                .FirstOrDefaultAsync(x => x.IdDocument == item);
            
            if(foundDocument ==null)
                continue;

            foundDocument.IsDeleted = true;
            _ef.Update(foundDocument);
            await _ef.SaveChangesAsync();
        }

        return Ok();
    }

    /// <summary>
    /// Получть документы пользователя
    /// </summary>
    /// <param name="token">Валидный токен</param>
    /// <param name="idAccount">ID аккаунта, которого получаем документы</param>
    /// <param name="showDeleted">Показывать удаленные документы</param>
    /// <returns>Массив документов</returns>
    [HttpGet("GetDocuments")]
    public async Task<IActionResult> GetDocuments([FromHeader] string token, long idAccount, bool showDeleted)
    {
        if (!IsValidateToken(token, out var session))
            return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

        if (session != null && !session.Account.AccountRole.CanViewDocuments)
            return StatusCode(403, ConfigMsg.NotAllowed);

        var documents = _ef.AccountsDocuments
            .Include(x => x.Account)
            .Where(x => x.Account != null && x.Account.IdAccount == idAccount && x.IsDeleted == showDeleted)
            .ToList();

        if (documents == null)
            return NotFound(ConfigMsg.ValidationElementNotFound);

        return Ok(documents);

    }

    
}*/