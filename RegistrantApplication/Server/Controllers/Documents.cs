using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistrantApplication.Server.Configs;
using RegistrantApplication.Server.Controllers.BaseAPI;
using RegistrantApplication.Server.Database;
using RegistrantApplication.Shared.API.AccountsDto;
using RegistrantApplication.Shared.Database.Accounts;

namespace RegistrantApplication.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class Documents : BaseApiController
{
    public Documents(ILogger<BaseApiController> logger, LiteContext ef, IMapper mapper) : base(logger, ef, mapper)
    {
    }
    
    /// <summary>
    /// Создать документ
    /// </summary>
    /// <param name="token">Валидный токен</param>
    /// <param name="form">Содержимое документа</param>
    /// <returns>Возращает 200 - если документ успешно сохранился</returns>
    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromHeader] string token, [FromBody] DocumentDto form)
    {
        if (!IsValidateToken(token, out var session))
            return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

        if (session != null && !session.Account.AccountRole.CanCreateDocuments)
            return StatusCode(403, ConfigMsg.NotAllowed);

        var foundAccount = await _ef.Accounts
            .FirstOrDefaultAsync(x => x.IdAccount == form.IdAcocunt);

        if (foundAccount == null)
            return NotFound(ConfigMsg.ValidationElementNotFound);

        var document = form.Adapt<Document>();
        document.Account = foundAccount;
        
        _ef.Add(form);
        _ef.Update(foundAccount);
        await _ef.SaveChangesAsync();
        
        return Ok(document.Adapt<DocumentDto>());
    }

    
    /// <summary>
    /// Обновление документа
    /// </summary>
    /// <param name="token">Валидный токен</param>
    /// <param name="form">Содержимое документа</param>
    /// <returns>Возращает 200 - если документ успешно сохранился</returns>
    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromHeader] string token, [FromBody] DocumentDto form)
    {
        if (!IsValidateToken(token, out var session))
            return Unauthorized(ConfigMsg.UnauthorizedInvalidToken);

        if (session != null && !session.Account.AccountRole.CanEditDocuments)
            return StatusCode(403, ConfigMsg.NotAllowed);

        var foundDocument = await _ef.AccountsDocuments
            .FirstOrDefaultAsync(x => x.IdDocument == form.IdDocument);

        if (foundDocument == null)
            return BadRequest(ConfigMsg.ValidationElementNotFound);

        foundDocument.Adapt(form);

        _ef.Update(foundDocument);
        await _ef.SaveChangesAsync();

        return Ok(foundDocument.Adapt<DocumentDto>());

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

        return Ok(documents.Adapt<List<DocumentDto>>());

    }
    
}